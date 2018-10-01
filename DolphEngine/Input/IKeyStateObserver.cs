namespace DolphEngine.Input
{
    public interface IKeyStateObserver
    {
        /// <summary>
        /// Performs any preliminary work to load the current state of all observed input devices.
        /// Runs once per Update iteration, before any <see cref="KeyState"/> objects are updated.
        /// </summary>
        /// <param name="gameTick">The current game tick to stamp on any KeyStates that have changed</param>
        void UpdateState(long gameTick);

        /// <summary>
        /// Updates the provided <see cref="KeyState"/> object, which represents the state of a single input dimension.
        /// Runs once per <see cref="KeyState"/> per Update iteration.
        /// </summary>
        /// <param name="keyState">The <see cref="KeyState"/> object to update</param>
        void UpdateKey(KeyState keyState);
    }
}
