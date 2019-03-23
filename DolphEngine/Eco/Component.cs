namespace DolphEngine.Eco
{
    public abstract class Component
    {
        #region Properties
        
        /// <summary>
        /// A reference to this component's <see cref="Eco.Entity"/>, if it has been added to one.
        /// </summary>
        public Entity Entity { get; internal set; }

        #endregion
    }
}