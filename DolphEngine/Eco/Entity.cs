using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Eco
{
    public class Entity
    {
        #region Constructors/Destructors

        /// <summary>
        /// Creates a new empty entity with no components.
        /// </summary>
        /// <param name="name">Optional. A short, descriptive name for the entity. A default name will be used if one is not provided.</param>
        public Entity(string name = null)
        {
            this.Id = ++_idCounter;
            this.Name = name ?? $"Entity {this.Id}";
        }

        private static uint _idCounter;

        #endregion

        #region Properties

        /// <summary>
        /// An auto-incrementing id number for this entity.
        /// </summary>
        public readonly uint Id;
        
        /// <summary>
        /// An descriptivate name for the entity, to help with logging or debugging.
        /// </summary>
        public readonly string Name;
        
        #endregion

        #region Components

        public IReadOnlyDictionary<Type, Component> ComponentsByType => this._componentsByType;
        private readonly Dictionary<Type, Component> _componentsByType = new Dictionary<Type, Component>();

        public Entity AddComponent<T>(T component) where T : Component
        {
            var type = typeof(T);

            if (component.Entity != null)
            {
                throw new InvalidOperationException($"Component '{type}' is already registered to Entity {component.Entity}");
            }
            
            if (this._componentsByType.ContainsKey(type))
            {
                throw new InvalidOperationException($"Entity '{this.Name}' ({this.Id}) already has component of type {type}");
            }

            component.Entity = this;
            this._componentsByType.Add(type, component);

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
            //this.Scene?.RegisterTag(this, tag);
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
            //this.Scene?.DeregisterTag(this, tag);
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
                throw new ArgumentException($"Entity '{this.Name}': No tags specified for HasAnyTags");
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
                throw new ArgumentException($"Entity '{this.Name}': No tags specified for HasAllTags");
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

        #region Object overrides

        public override string ToString()
        {
            return $"{{ id: {this.Id}, name: {this.Name} }}";
        }

        #endregion
    }
}