using System;
using System.Collections.Generic;

namespace TacticsGame.Engine.Eco
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
}
