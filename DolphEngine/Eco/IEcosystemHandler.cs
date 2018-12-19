using System;
using System.Collections.Generic;

namespace DolphEngine.Eco
{
    public interface IEcosystemHandler
    {
        /// <summary>
        /// An enumerable of the <see cref="Component"/> types to which this handler subscribes.
        /// In other words, any <see cref="Entity"/> in the <see cref="Ecosystem"/> that has all of the 
        /// <see cref="Component"/> types in this list will be in the collection of entities passed to the 
        /// <see cref="Handle"/> method.
        /// </summary>
        /// <throws>
        /// <see cref="ArgumentException"/> when this handler is added to an <see cref="Ecosystem"/> if this
        /// property is null or empty, or if any <see cref="Type"/> contained within does not implement
        /// <see cref="Component"/>.
        /// </throws>
        IEnumerable<Type> SubscribesTo { get; }

        /// <summary>
        /// An operation to perform on a group of <see cref="Entity"/> objects that have specific <see cref="Component"/>s.
        /// </summary>
        /// <param name="entities">
        /// A collection of <see cref="Entity"/> objects within the current <see cref="Ecosystem"/>. Each one
        /// is guaranteed to have had the <see cref="Component"/> types designated in <see cref="SubscribesTo"/>
        /// at start of the current handler iteration.
        /// </param>
        void Handle(IEnumerable<Entity> entities);
    }

    public abstract class EcosystemHandler<TComponent> : IEcosystemHandler
        where TComponent : Component
    {
        /// <inheritdoc/>
        public IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent) };

        /// <inheritdoc/>
        public abstract void Handle(IEnumerable<Entity> entities);
    }

    public abstract class EcosystemHandler<TComponent1, TComponent2> : IEcosystemHandler
        where TComponent1 : Component
        where TComponent2 : Component
    {
        /// <inheritdoc/>
        public IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent1), typeof(TComponent2) };

        /// <inheritdoc/>
        public abstract void Handle(IEnumerable<Entity> entities);
    }

    public abstract class EcosystemHandler<TComponent1, TComponent2, TComponent3> : IEcosystemHandler
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
    {
        /// <inheritdoc/>
        public IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent1), typeof(TComponent2), typeof(TComponent3) };

        /// <inheritdoc/>
        public abstract void Handle(IEnumerable<Entity> entities);
    }

    public abstract class EcosystemHandler<TComponent1, TComponent2, TComponent3, TComponent4> : IEcosystemHandler
        where TComponent1 : Component
        where TComponent2 : Component
        where TComponent3 : Component
        where TComponent4 : Component
    {
        /// <inheritdoc/>
        public IEnumerable<Type> SubscribesTo => new[] { typeof(TComponent1), typeof(TComponent2), typeof(TComponent3), typeof(TComponent4) };

        /// <inheritdoc/>
        public abstract void Handle(IEnumerable<Entity> entities);
    }
}
