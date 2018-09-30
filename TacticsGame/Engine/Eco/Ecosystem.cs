using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Engine.Eco.Components;

namespace TacticsGame.Engine.Eco
{
    public sealed class Ecosystem
    {
        #region Data structures

        // All entities added to the ecosystem, indexed by id
        private readonly Dictionary<uint, Entity> _entitiesById = new Dictionary<uint, Entity>();

        // All instances of components on all entities across the ecosystem
        // todo: might not need this, handlers and bitlocks might present a better solution
        private readonly HashSet<Component> _allComponents = new HashSet<Component>(ReferenceEqualityComparer<Component>.Instance);

        // All components in the ecosystem indexed by their derived type
        // todo: might not need this, handlers and bitlocks might present a better solution
        private readonly Dictionary<Type, HashSet<Component>> _componentsByType = new Dictionary<Type, HashSet<Component>>();

        // All types of components for which at least one handler has been registered to this ecosystem at some point, paired with the
        // bitPosition by which the type is represented in a BitLock or BitKey
        private readonly Dictionary<Type, ushort> _componentBitPositions = new Dictionary<Type, ushort>();

        // All BitLocks that have been created to index entities and handlers, indexed by the individual bitPositions (which represent
        // component types) that they support. This speeds up indexing add/remove components on entities.
        private readonly Dictionary<ushort, HashSet<BitLock>> _locksByBits = new Dictionary<ushort, HashSet<BitLock>>();

        // All handlers that have been registered to the ecosystem, paired with the BitLock which represents
        // the components that the handler subscribes to. A handler's BitLock/Subscriptions never change throughout the
        // lifetime of the application.
        private readonly Dictionary<IEcosystemHandler, BitLock> _locksByHandler = new Dictionary<IEcosystemHandler, BitLock>(ReferenceEqualityComparer<IEcosystemHandler>.Instance);

        // All entities in the ecosystem, indexed by the BitLocks for handlers within the Ecosystem that they support.
        // In other words, all of the entities that have a PositionComponent are grouped together; all the entities that have
        // a PositionComponent _and_ a SpriteComponent are groupd together, etc. This makes getting the arguments for a handler an O(1) operation.
        private readonly Dictionary<BitLock, HashSet<Entity>> _entitiesByLock = new Dictionary<BitLock, HashSet<Entity>>(ReferenceEqualityComparer<BitLock>.Instance);
        private readonly Dictionary<uint, HashSet<BitLock>> _locksByEntity = new Dictionary<uint, HashSet<BitLock>>();

        // A pairing of all entities in the ecosystem with a BitKey that represents the components they currently have.
        // If the BitKey is null, then the entity's components have changed and its BitKey needs to be regenerated.
        private readonly Dictionary<uint, BitKey> _bitKeysByEntityId = new Dictionary<uint, BitKey>();

        // If an entity within the ecosystem has a component added or removed, its BitKey will need to be refreshed
        // before we can know which BitLocks (handlers) it should be sent to.
        private readonly HashSet<Entity> _entitiesToRefreshBitKey = new HashSet<Entity>(ReferenceEqualityComparer<Entity>.Instance);

        // todo: experimental, might delete these
        private readonly Dictionary<PositionComponent, Tuple<int, int>> _positionComponentCoordinates = new Dictionary<PositionComponent, Tuple<int, int>>(ReferenceEqualityComparer<PositionComponent>.Instance);
        private readonly SortedDictionary<int, SortedDictionary<int, HashSet<PositionComponent>>> _positionComponentsByPosition = new SortedDictionary<int, SortedDictionary<int, HashSet<PositionComponent>>>();

        private ushort _nextBitPosition;

        #endregion

        #region Handler methods
        
        public Ecosystem AddHandler(IEcosystemHandler handler)
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

            return this;
        }

        public Ecosystem AddHandler<T>() where T : IEcosystemHandler, new() => this.AddHandler(new T());

        public Ecosystem AddHandlers(IEnumerable<IEcosystemHandler> handlers)
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

        public Ecosystem AddHandlers(params IEcosystemHandler[] handlers)
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

        public Ecosystem RemoveHandler(IEcosystemHandler handler)
        {
            if (!this._locksByHandler.TryGetValue(handler, out var bitLock))
            {
                // The handler wasn't registered in the first place
                return this;
            }

            this._locksByHandler.Remove(handler);
            this._entitiesByLock.Remove(bitLock);
            this.DeindexBitLock(bitLock);

            return this;
        }

        public Ecosystem RemoveHandlers(IEnumerable<IEcosystemHandler> handlers)
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

        public Ecosystem RemoveHandlers(params IEcosystemHandler[] handlers)
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
            this._locksByHandler.Clear();
            this._entitiesByLock.Clear();
            this._locksByBits.Clear();

            return this;
        }

        public void RunAllHandlers()
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
                lockByHandler.Key.Handle(entitiesByThisLock);
            }
        }

        #endregion

        #region Entity methods

        public Ecosystem AddEntity(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (this._entitiesById.ContainsKey(entity.Id))
            {
                throw new ArgumentException($"Entity '{entity.Name}' ({entity.Id}) has already been added to this {nameof(Ecosystem)}!");
            }

            // Index the entity by its id
            this._entitiesById.Add(entity.Id, entity);
            
            // Index the entity by its BitKey
            this.CreateAndIndexBitKey(entity);

            entity.Ecosystem = this;
            return this;
        }        

        public Ecosystem AddEntities(IEnumerable<Entity> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException(nameof(entities), "There are no entities to add!");
            }

            foreach (var entity in entities)
            {
                this.AddEntity(entity);
            }

            return this;
        }

        public Ecosystem AddEntities(params Entity[] entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException(nameof(entities), "There are no entities to add!");
            }

            foreach (var entity in entities)
            {
                this.AddEntity(entity);
            }

            return this;
        }

        public Entity GetEntity(uint id)
        {
            return this._entitiesById[id];
        }

        public bool TryGetEntity(uint id, out Entity entity)
        {
            if (!this._entitiesById.TryGetValue(id, out var indexedEntity))
            {
                entity = null;
                return false;
            }

            entity = indexedEntity;
            return true;
        }

        public Ecosystem RemoveEntity(Entity entity)
        {
            var id = entity.Id;
            if (!this._entitiesById.ContainsKey(id))
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

            entity.Ecosystem = null;
            return this;
        }

        public Ecosystem RemoveEntities(IEnumerable<Entity> entities)
        {
            if (entities == null || !entities.Any())
            {
                return this;
            }

            foreach (var entity in entities)
            {
                this.RemoveEntity(entity);
            }

            return this;
        }

        public Ecosystem RemoveEntities(params Entity[] entities)
        {
            if (entities == null || !entities.Any())
            {
                return this;
            }

            foreach (var entity in entities)
            {
                this.RemoveEntity(entity);
            }

            return this;
        }

        #endregion

        #region Component methods

        public IEnumerable<T> GetComponentsByType<T>() where T : Component
        {
            if (!this._componentsByType.TryGetValue(typeof(T), out var componentsByThisType))
            {
                return Enumerable.Empty<T>();
            }

            return componentsByThisType.Select(x => (T)x);
        }

        public IEnumerable<Entity> GetEntitiesWithComponent<T>() where T : Component
        {
            var componentsByThisType = this.GetComponentsByType<T>();
            if (!componentsByThisType.Any())
            {
                return Enumerable.Empty<Entity>();
            }

            return componentsByThisType.Select(c => c.Entity); // todo: optimize with new index, or filter out dupe entities
        }

        public bool TryGetComponentForEntity<T>(uint id, out T component) where T : Component
        {
            if (!this.TryGetEntity(id, out var indexedEntity))
            {
                component = null;
                return false;
            }

            if (!indexedEntity.TryGetComponent<T>(out var indexedComponent))
            {
                component = null;
                return false;
            }

            component = indexedComponent;
            return true;
        }

        //public IEnumerable<PositionComponent> GetPositionComponentsInArea(int x, int y, int width, int height)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Entity> GetEntitiesInArea(int x, int y, int width, int height)
        //{
        //    var positionComponents = this.GetPositionComponentsInArea(x, y, width, height);
        //    if (!positionComponents.Any())
        //    {
        //        return Enumerable.Empty<Entity>();
        //    }

        //    return positionComponents.Select(c => c.Entity);
        //}

        #endregion

        #region Notifications (internal)
        
        internal void NotifyComponentAttached<T>(Entity entity, T component) where T : Component
        {
            // If it's already been indexed, quit now
            if (this._allComponents.Contains(component))
            {
                return;
            }
            this._allComponents.Add(component);

            // Index the component by type
            var type = typeof(T);
            if (!this._componentsByType.TryGetValue(type, out var componentsByThisType))
            {
                componentsByThisType = new HashSet<Component>(ReferenceEqualityComparer<Component>.Instance);
                this._componentsByType.Add(type, componentsByThisType);
            }
            componentsByThisType.Add(component);

            // Mark that this entity needs its BitKey refreshed before the next handler run
            this._entitiesToRefreshBitKey.Add(entity);
            
            //if (component is PositionComponent positionComponent)
            //{
            //    var x = positionComponent.X;
            //    var y = positionComponent.Y;

            //    // Index the component by its current x,y position
            //    if (!this._positionComponentsByPosition.TryGetValue(x, out var componentsAtThisX))
            //    {
            //        componentsAtThisX = new SortedDictionary<int, HashSet<PositionComponent>>();
            //        this._positionComponentsByPosition.Add(x, componentsAtThisX);
            //    }
            //    if (!componentsAtThisX.TryGetValue(y, out var componentsAtThisXY))
            //    {
            //        componentsAtThisXY = new HashSet<PositionComponent>(ReferenceEqualityComparer<PositionComponent>.Instance);
            //        componentsAtThisX.Add(y, componentsAtThisXY);
            //    }
            //    componentsAtThisXY.Add(positionComponent);

            //    // Add reverse-reference to see which coordinates this component is currently indexed at
            //    this._positionComponentCoordinates.Add(positionComponent, new Tuple<int, int>(x, y));
            //}
        }

        internal void NotifyComponentDetached<T>(Entity entity, T component) where T : Component
        {
            if (!this._allComponents.Contains(component))
            {
                return;
            }
            this._allComponents.Remove(component);

            // Remove the component from the type index
            var type = typeof(T);
            var componentsByThisType = this._componentsByType[type];
            componentsByThisType.Remove(component);
            if (!componentsByThisType.Any())
            {
                this._componentsByType.Remove(type);
            }

            // Mark that this entity needs its BitKey refreshed before the next handler run
            this._entitiesToRefreshBitKey.Add(entity);

            //if (component is PositionComponent positionComponent)
            //{
            //    var x = positionComponent.X;
            //    var y = positionComponent.Y;

            //    // Remove the component from its x,y position index
            //    var componentsAtThisX = this._positionComponentsByPosition[x];
            //    var componentsAtThisXY = componentsAtThisX[y];
            //    componentsAtThisXY.Remove(positionComponent);
            //    if (!componentsAtThisXY.Any())
            //    {
            //        componentsAtThisX.Remove(y);
            //    }
            //    if (!componentsAtThisX.Any())
            //    {
            //        this._positionComponentsByPosition.Remove(x);
            //    }

            //    // Remove its reverse-reference as well
            //    this._positionComponentCoordinates.Remove(positionComponent);
            //}
        }

        //internal static void NotifyPositionComponentChanged(PositionComponent positionComponent)
        //{
        //    throw new NotImplementedException();
        //}

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
    }
}
