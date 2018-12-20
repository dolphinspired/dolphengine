using System;
using System.Collections.Generic;

namespace DolphEngine.Eco
{
    public abstract class EcosystemHandler
    {
        /// <summary>
        /// An enumerable of the <see cref="Component"/> types to which this handler subscribes.
        /// In other words, any <see cref="Entity"/> in the <see cref="Ecosystem"/> that has all of the 
        /// <see cref="Component"/> types in this list will be in the collection of entities passed to the 
        /// <see cref="Update(IEnumerable{Entity})"/> and <see cref="Draw(IEnumerable{Entity})"/> methods.
        /// </summary>
        /// <throws>
        /// <see cref="ArgumentException"/> when this handler is added to an <see cref="Ecosystem"/> if this
        /// property is null or empty, or if any <see cref="Type"/> contained within does not implement
        /// <see cref="Component"/>.
        /// </throws>
        public abstract IEnumerable<Type> SubscribesTo { get; }

        /// <summary>
        /// An update operation to perform on a group of entities. Each entity supplied in the parameter is guaranteed
        /// to have all of the <see cref="Component"/> types designated in the  <see cref="SubscribesTo"/>
        /// at start of the current handler iteration.
        /// </summary>
        /// <param name="entities">
        /// A collection of <see cref="Entity"/> objects within the current <see cref="Ecosystem"/>.
        /// </param>
        public virtual void Update(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                this.Update(entity);
            }
        }

        /// <summary>
        /// An update operation to perform on an entity. The entity supplied in the parameter is guaranteed
        /// to have all of the <see cref="Component"/> types designated in the  <see cref="SubscribesTo"/>
        /// at start of the current handler iteration.
        /// </summary>
        /// <param name="entities">
        /// An <see cref="Entity"/> object within the current <see cref="Ecosystem"/>.
        /// </param>
        public virtual void Update(Entity entity)
        {
        }

        /// <summary>
        /// A draw operation to perform on a group of entities. Each entity supplied in the parameter is guaranteed
        /// to have all of the <see cref="Component"/> types designated in the  <see cref="SubscribesTo"/>
        /// at start of the current handler iteration.
        /// </summary>
        /// <param name="entities">
        /// A collection of <see cref="Entity"/> objects within the current <see cref="Ecosystem"/>.
        /// </param>
        public virtual void Draw(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                this.Draw(entity);
            }
        }

        /// <summary>
        /// A draw operation to perform on an entity. The entity supplied in the parameter is guaranteed
        /// to have all of the <see cref="Component"/> types designated in the  <see cref="SubscribesTo"/>
        /// at start of the current handler iteration.
        /// </summary>
        /// <param name="entities">
        /// An <see cref="Entity"/> object within the current <see cref="Ecosystem"/>.
        /// </param>
        public virtual void Draw(Entity entity)
        {
        }
    }

    public abstract class EcosystemHandler<TComponent> : EcosystemHandler
        where TComponent : Component
    {
        /// <inheritdoc/>
        public sealed override IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent) };
    }

    public abstract class EcosystemHandler<TComponent1, TComponent2> : EcosystemHandler
        where TComponent1 : Component
        where TComponent2 : Component
    {
        /// <inheritdoc/>
        public sealed override IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent1), typeof(TComponent2) };
    }

    public abstract class EcosystemHandler<TComponent1, TComponent2, TComponent3> : EcosystemHandler
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
    {
        /// <inheritdoc/>
        public sealed override IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent1), typeof(TComponent2), typeof(TComponent3) };
    }

    public abstract class EcosystemHandler<TComponent1, TComponent2, TComponent3, TComponent4> : EcosystemHandler
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
        where TComponent4 : Component
    {
        /// <inheritdoc/>
        public sealed override IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4) };
    }
}
