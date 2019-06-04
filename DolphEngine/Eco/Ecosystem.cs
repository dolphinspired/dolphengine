using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Eco
{
    public class Ecosystem
    {
        #region Constructors

        public Ecosystem(IGameTimer timer)
        {
            this._timer = timer;
        }
        
        private readonly IGameTimer _timer;

        #endregion

        #region Data structures

        // All entities added to the ecosystem, indexed by id
        private readonly Dictionary<string, Entity> _entitiesById = new Dictionary<string, Entity>();
        
        // All types of components for which at least one handler has been registered to this ecosystem at some point, paired with the
        // bitPosition by which the type is represented in a BitLock or BitKey
        private readonly Dictionary<Type, ushort> _componentBitPositions = new Dictionary<Type, ushort>();

        // All BitLocks that have been created to index entities and handlers, indexed by the individual bitPositions (which represent
        // component types) that they support. This speeds up indexing add/remove components on entities.
        private readonly Dictionary<ushort, HashSet<BitLock>> _locksByBits = new Dictionary<ushort, HashSet<BitLock>>();

        // All handlers that have been registered to the ecosystem, paired with the BitLock which represents
        // the components that the handler subscribes to. A handler's BitLock/Subscriptions never change throughout the
        // lifetime of the application.
        private readonly Dictionary<EcosystemHandler, BitLock> _locksByHandler = new Dictionary<EcosystemHandler, BitLock>(ReferenceEqualityComparer<EcosystemHandler>.Instance);

        // All entities in the ecosystem, indexed by the BitLocks for handlers within the Ecosystem that they support.
        // In other words, all of the entities that have a PositionComponent are grouped together; all the entities that have
        // a PositionComponent _and_ a SpriteComponent are groupd together, etc. This makes getting the arguments for a handler an O(1) operation.
        private readonly Dictionary<BitLock, HashSet<Entity>> _entitiesByLock = new Dictionary<BitLock, HashSet<Entity>>(ReferenceEqualityComparer<BitLock>.Instance);
        private readonly Dictionary<string, HashSet<BitLock>> _locksByEntity = new Dictionary<string, HashSet<BitLock>>();

        // A pairing of all entities in the ecosystem with a BitKey that represents the components they currently have.
        // If the BitKey is null, then the entity's components have changed and its BitKey needs to be regenerated.
        private readonly Dictionary<string, BitKey> _bitKeysByEntityId = new Dictionary<string, BitKey>();

        // If an entity within the ecosystem has a component added or removed, its BitKey will need to be refreshed
        // before we can know which BitLocks (handlers) it should be sent to.
        private readonly HashSet<Entity> _entitiesToRefreshBitKey = new HashSet<Entity>(ReferenceEqualityComparer<Entity>.Instance);

        private ushort _nextBitPosition;

        #endregion

        #region Handler methods
        
        public Ecosystem AddHandler(EcosystemHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (handler.SubscribesTo == null || !handler.SubscribesTo.Any())
            {
                throw new ArgumentException($"At least one component must be subscribed to in {handler.GetType()}!");
            }

            if (this._locksByHandler.ContainsKey(handler))
            {
                throw new ArgumentException($"{handler.GetType()} has already been added!");
            }

            // Collect a set of bitPositions that will be used to build a BitLock for this type combination
            var bitPositions = new List<ushort>();
            foreach (var type in handler.SubscribesTo)
            {
                var bitPosition = this.GetOrAssignBitPosition(type);
                bitPositions.Add(bitPosition);
            }

            // Create a BitLock for the handler, and store the handler along with its BitLock
            var bitLock = this.CreateAndIndexBitLock(bitPositions);
            this._locksByHandler.Add(handler, bitLock);

            // Next, find all the entities currently in the Ecosystem that have the components that will fit this lock,
            // then index them by this lock. So when the handler says "give me all the entities I need to handle", we
            // already have that list cached.
            var entitiesByThisLock = new HashSet<Entity>(ReferenceEqualityComparer<Entity>.Instance);
            this._entitiesByLock.Add(bitLock, entitiesByThisLock);
            if (this._entitiesById.Any())
            {
                foreach (var entityById in this._entitiesById)
                {
                    if (entityById.Value.HasAllComponents(handler.SubscribesTo.ToArray())) // todo: separate method overload for HasAllComponents?
                    {
                        entitiesByThisLock.Add(entityById.Value);
                    }
                }
            }

            handler._timer = this._timer;
            return this;
        }

        public Ecosystem AddHandler<T>() where T : EcosystemHandler, new() => this.AddHandler(new T());

        public Ecosystem AddHandlers(IEnumerable<EcosystemHandler> handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                throw new ArgumentException("There are no handlers to add!");
            }

            foreach (var handler in handlers)
            {
                this.AddHandler(handler);
            }

            return this;
        }

        public Ecosystem AddHandlers(params EcosystemHandler[] handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                throw new ArgumentException("There are no handlers to add!");
            }

            foreach (var handler in handlers)
            {
                this.AddHandler(handler);
            }

            return this;
        }

        public Ecosystem RemoveHandler(EcosystemHandler handler)
        {
            if (!this._locksByHandler.TryGetValue(handler, out var bitLock))
            {
                // The handler wasn't registered in the first place
                return this;
            }

            this._locksByHandler.Remove(handler);
            this._entitiesByLock.Remove(bitLock);
            this.DeindexBitLock(bitLock);

            handler._timer = null;
            return this;
        }

        public Ecosystem RemoveHandlers(IEnumerable<EcosystemHandler> handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                return this;
            }

            foreach (var handler in handlers)
            {
                this.RemoveHandler(handler);
            }

            return this;
        }

        public Ecosystem RemoveHandlers(params EcosystemHandler[] handlers)
        {
            if (handlers == null || !handlers.Any())
            {
                return this;
            }

            foreach (var handler in handlers)
            {
                this.RemoveHandler(handler);
            }

            return this;
        }

        public Ecosystem ClearHandlers()
        {
            foreach (var handler in this._locksByHandler.Keys)
            {
                handler._timer = null;
            }

            this._locksByHandler.Clear();
            this._entitiesByLock.Clear();
            this._locksByBits.Clear();

            return this;
        }

        public void Update()
        {
            if (this._entitiesToRefreshBitKey.Any())
            {
                foreach (var entity in this._entitiesToRefreshBitKey)
                {
                    this.CreateAndIndexBitKey(entity);
                }

                this._entitiesToRefreshBitKey.Clear();
            }

            foreach (var lockByHandler in this._locksByHandler)
            {
                var entitiesByThisLock = this._entitiesByLock[lockByHandler.Value];
                lockByHandler.Key.Update(entitiesByThisLock);
            }
        }

        public void Draw()
        {
            foreach (var lockByHandler in this._locksByHandler)
            {
                var entitiesByThisLock = this._entitiesByLock[lockByHandler.Value];
                lockByHandler.Key.Draw(entitiesByThisLock);
            }
        }

        #endregion

        #region Entity methods

        public Ecosystem AddEntity(string id, Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Id != null)
            {
                throw new InvalidOperationException($"Cannot add entity with id '{id}', it has already been assigned id '{entity.Id}'!");
            }

            if (this._entitiesById.ContainsKey(id))
            {
                throw new InvalidOperationException($"An entity with id '{id}' has already been added to this {nameof(Ecosystem)}!");
            }

            // Index the entity by its id
            this._entitiesById.Add(id, entity);
            entity.Id = id;

            // Index the entity by its BitKey
            this.CreateAndIndexBitKey(entity);

            entity.Ecosystem = this;
            return this;
        }        

        public Ecosystem AddEntities(Func<int, string> idBuilder, IEnumerable<Entity> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException(nameof(entities), "There are no entities to add!");
            }

            var i = 0;
            foreach (var entity in entities)
            {
                this.AddEntity(idBuilder(i++), entity);
            }

            return this;
        }

        public Entity GetEntity(string id)
        {
            return this._entitiesById[id];
        }

        public IEnumerable<Entity> GetEntities()
        {
            return this._entitiesById.Select(x => x.Value);
        }

        public bool TryGetEntity(string id, out Entity entity)
        {
            if (!this._entitiesById.TryGetValue(id, out var indexedEntity))
            {
                entity = null;
                return false;
            }

            entity = indexedEntity;
            return true;
        }

        public Ecosystem RemoveEntity(string id)
        {
            if (!this._entitiesById.TryGetValue(id, out var entity))
            {
                return this;
            }

            this._entitiesById.Remove(id);
            this._bitKeysByEntityId.Remove(id);

            var bitLocks = this._locksByEntity[id];
            this._locksByEntity.Remove(id);
            foreach (var bitLock in bitLocks)
            {
                this._entitiesByLock[bitLock].Remove(entity);
            }

            entity.Id = null;
            entity.Ecosystem = null;
            return this;
        }

        public Ecosystem RemoveEntities(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return this;
            }

            foreach (var id in ids)
            {
                this.RemoveEntity(id);
            }

            return this;
        }

        #endregion

        #region BitKey/BitLock management (private)

        private BitLock CreateAndIndexBitLock(IEnumerable<ushort> bitPositions)
        {
            var bitLock = new BitLock(bitPositions);

            foreach (var bitPosition in bitPositions)
            {
                if (!this._locksByBits.TryGetValue(bitPosition, out var indexedLocks))
                {
                    indexedLocks = new HashSet<BitLock>(ReferenceEqualityComparer<BitLock>.Instance);
                    this._locksByBits.Add(bitPosition, indexedLocks);
                }

                indexedLocks.Add(bitLock);
            }

            return bitLock;
        }

        private void DeindexBitLock(BitLock bitLock)
        {
            if (bitLock == null)
            {
                return;
            }

            foreach (var bitPosition in bitLock.BitPositions.Distinct())
            {
                var bitLocks = this._locksByBits[bitPosition];
                bitLocks.Remove(bitLock);

                if (!bitLocks.Any())
                {
                    this._locksByBits.Remove(bitPosition);
                }
            }
        }

        private ushort GetOrAssignBitPosition(Type type)
        {
            if (!this._componentBitPositions.TryGetValue(type, out var bitPosition))
            {
                // If this component type has not been assigned a bitPosition, give it one now
                if (!typeof(Component).IsAssignableFrom(type))
                {
                    throw new ArgumentException($"Cannot assign bit position to {type.Name}. It does not inherit from {nameof(Component)}!");
                }

                bitPosition = this._nextBitPosition++;
                this._componentBitPositions.Add(type, bitPosition);
            }

            return bitPosition;
        }

        private BitKey CreateAndIndexBitKey(Entity entity)
        {
            // Give the entity a BitKey and index that key by the entity's id
            var bitKey = this.CreateBitKey(entity);
            if (this._bitKeysByEntityId.ContainsKey(entity.Id))
            {
                this._bitKeysByEntityId[entity.Id] = bitKey;
            }
            else
            {
                this._bitKeysByEntityId.Add(entity.Id, bitKey);
            }            

            // Try this new key in all of the known locks
            // Index the entity by all the locks that we know it it fits
            var locksByThisEntityId = new HashSet<BitLock>(ReferenceEqualityComparer<BitLock>.Instance);
            if (this._locksByEntity.ContainsKey(entity.Id))
            {
                this._locksByEntity[entity.Id] = locksByThisEntityId;
            }
            else
            {
                this._locksByEntity.Add(entity.Id, locksByThisEntityId);
            }
            
            foreach (var entityByLock in this._entitiesByLock)
            {
                var bitLock = entityByLock.Key;
                if (bitKey.Unlocks(bitLock))
                {
                    // Add this entity to a list of entities that can unlock this lock
                    entityByLock.Value.Add(entity);

                    // Add this lock to the known locks that this entity can unlock
                    locksByThisEntityId.Add(bitLock);
                }
                else
                {
                    // Remove this entity from the list of entities that can unlock this lock (if it's present)
                    entityByLock.Value.Remove(entity);

                    // Remove this lock from the known locks that this entity can unlock (if it's present)
                    locksByThisEntityId.Remove(bitLock);
                }
            }

            return bitKey;
        }

        private BitKey CreateBitKey(Entity entity)
        {
            BitKey bitKey;
            var entityComponentsByType = entity.ComponentsByType;
            if (entityComponentsByType.Any())
            {
                var bitPositions = new List<ushort>();
                foreach (var entityComponentByType in entityComponentsByType)
                {
                    var type = entityComponentByType.Key;
                    var bitPosition = this.GetOrAssignBitPosition(type);
                    bitPositions.Add(bitPosition);
                }

                bitKey = new BitKey(bitPositions);
            }
            else
            {
                // If the entity has no components, create an empty BitKey
                bitKey = new BitKey(null);
            }

            return bitKey;
        }

        #endregion

        #region Notifications (internal)

        internal void NotifyChanged(Entity entity)
        {
            // Mark that this entity needs its BitKey refreshed before the next handler run	
            this._entitiesToRefreshBitKey.Add(entity);
        }

        #endregion
    }
}
