using DolphEngine.Input.State;

namespace DolphEngine.Input
{
    public interface IKeyStateObserver
    {
        /// <summary>
        /// Run once per frame.
        /// </summary>
        void UpdateState();
        
        /// <summary>
        /// Run once per key per frame.
        /// </summary>
        /// <param name="key">The generic key to look up</param>
        object GetKeyValue(InputKey key);
    }
}
