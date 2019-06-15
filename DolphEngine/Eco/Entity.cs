using DolphEngine.Graphics.Directives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Eco
{
    public class Entity
    {
        #region Constructors/Destructors

        /// <summary>
        /// Creates an entity.
        /// </summary>
        public Entity()
        {
        }

        /// <summary>
        /// Creates an entity and places it at the provided position.
        /// </summary>
        public Entity(Position2d position)
        {
            this.Space = new Rect2d(position, Size2d.Zero);
        }

        /// <summary>
        /// Creates an entity and places it at the provided position with the given size.
        /// </summary>
        public Entity(Rect2d space)
        {
            this.Space = space;
        }

        #endregion

        #region Information
        
        /// <summary>	
        /// A reference to this entity's <see cref="Ecosystem"/>, if it has been added to one.	
        /// </summary>	
        public Ecosystem Ecosystem { get; internal set; }

        /// <summary>
        /// Each entity is assigned an Id when it's added to an <see cref="Ecosystem"/>.
        /// </summary>
        public string Id { get; internal set; }

        #endregion

        #region Core data

        public Rect2d Space;

        #endregion

        #region Components

        public IReadOnlyDictionary<Type, Component> ComponentsByType => this._componentsByType;
        private readonly Dictionary<Type, Component> _componentsByType = new Dictionary<Type, Component>();

        public Entity AddComponent<T>(T component) where T : Component
        {
            var type = typeof(T);

            if (component.Entity != null)
            {
                throw new InvalidOperationException($"Component '{type}' is already registered to Entity {component.Entity}!");
            }
            
            if (this._componentsByType.ContainsKey(type))
            {
                throw new InvalidOperationException($"Entity already has component of type {type}!");
            }

            component.Entity = this;
            this._componentsByType.Add(type, component);

            this.Ecosystem?.NotifyChanged(this);
            return this;
        }

        public Entity AddComponent<T>() where T : Component, new() => this.AddComponent(new T());

        public Entity RemoveComponent<T>() where T : Component
        {
            if (!this.TryGetComponent<T>(out var component))
            {
                return this;
            }

            component.Entity = null;
            this._componentsByType.Remove(typeof(T));

            this.Ecosystem?.NotifyChanged(this);
            return this;
        }

        public Entity RemoveComponent<T>(T component) where T : Component => this.RemoveComponent<T>();

        public T GetComponent<T>() where T : Component
        {
            return this._componentsByType[typeof(T)] as T;
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            if (!this._componentsByType.TryGetValue(typeof(T), out Component indexedComponent))
            {
                component = null;
                return false;
            }

            component = indexedComponent as T;
            return true;
        }

        public T GetComponentOrDefault<T>(T defaultComponent = default(T)) where T : Component
        {
            if (this.TryGetComponent<T>(out var component))
            {
                return component;
            }

            return defaultComponent;
        }

        public bool HasComponents()
        {
            return this._componentsByType.Any();
        }

        public bool HasComponent<T>() where T : Component
        {
            return this._componentsByType.ContainsKey(typeof(T));
        }

        public bool HasAnyComponents(params Type[] types)
        {
            return types.Any(this._componentsByType.ContainsKey);
        }

        public bool HasAllComponents(params Type[] types)
        {
            return types.All(this._componentsByType.ContainsKey);
        }

        #endregion

        #region Tags

        public IReadOnlyCollection<string> Tags => _tags;
        private HashSet<string> _tags = new HashSet<string>(0);

        public Entity AddTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return this;
            }

            this._tags.Add(tag);
            return this;
        }

        public Entity AddTags(params string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                return this;
            }

            foreach (var tag in tags)
            {
                this.AddTag(tag);
            }

            return this;
        }

        public Entity RemoveTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return this;
            }

            this._tags.Remove(tag);
            return this;
        }

        public Entity RemoveTags(params string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                return this;
            }

            foreach (var tag in tags)
            {
                this.RemoveTag(tag);
            }

            return this;
        }

        public bool HasTag(string tag)
        {
            return this._tags.Contains(tag);
        }

        public bool HasAnyTags(params string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                throw new ArgumentException($"Entity: No tags specified for HasAnyTags!");
            }

            foreach (var tag in tags)
            {
                if (this._tags.Contains(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAllTags(params string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                throw new ArgumentException($"Entity: No tags specified for HasAllTags!");
            }

            foreach (var tag in tags)
            {
                if (!this._tags.Contains(tag))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Drawing

        public Dictionary<string, DrawDirective> DrawDirectives { internal get; set; } = new Dictionary<string, DrawDirective>();

        public Entity SetDirective<TDirective>(string name, Action<TDirective> action)
            where TDirective : DrawDirective, new()
        {
            if (!this.DrawDirectives.TryGetValue(name, out var directive))
            {
                var typed = new TDirective();
                action(typed);
                this.DrawDirectives.Add(name, typed);
            }
            else
            {
                var typed = directive as TDirective;
                if (typed == null)
                {
                    throw new InvalidCastException($"Attempted to set directive '{name}' with an action for type '{typeof(TDirective).Name}' on entity '{this.Id}', " +
                        $"but a directive already exists by this name with type '{directive.GetType().Name}'!");
                }
                action(typed);
            }

            return this;
        }

        public Entity RemoveDirective(string name)
        {
            this.DrawDirectives.Remove(name);
            return this;
        }

        public Entity ClearDirectives()
        {
            this.DrawDirectives.Clear();
            return this;
        }

        #endregion

        #region Object overrides

        public override string ToString()
        {
            return $"{{ id: {Id}, components: {_componentsByType.Count}, space: {Space} }}";
        }

        #endregion
    }
}