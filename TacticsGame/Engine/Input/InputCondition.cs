using System;

namespace TacticsGame.Engine.Input
{
    /// <summary>
    /// Specifies a <see cref="KeyState"/> condition under which a reaction should be run.
    /// </summary>
    [Flags]
    public enum InputCondition
    {
        /// <summary>
        /// A custom evaluation of the <see cref="KeyState"/> determines whether or not the reaction should
        /// be called. Do not set this status directly; use the <see cref="KeyHandler"/> overload that requires
        /// a function as a parameter instead.
        /// </summary>
        Custom = 0,

        /// <summary>
        /// The reaction will called on every update, regardless of the current <see cref="KeyState"/>.
        /// </summary>
        Always = 1 << 1,

        /// <summary>
        /// The reaction will be called once each time the input is pressed.
        /// </summary>
        WhenPressed = 1 << 2,

        /// <summary>
        /// The reaction will be called continuously as long as the input is pressed.
        /// </summary>
        WhilePressed = 1 << 3,

        /// <summary>
        /// The reaction will be called once each time the input is released.
        /// </summary>
        WhenReleased = 1 << 4,

        /// <summary>
        /// The reaction will be called continuously as long as the input is *not* pressed.
        /// </summary>
        WhileReleased = 1 << 5,

        /// <summary>
        /// The reaction will be called once each time the input's digital values change.
        /// </summary>
        WhenDigitalChanged = 1 << 6,

        /// <summary>
        /// The reaction will be called once each time the input's analog values change.
        /// </summary>
        WhenAnalogChanged = 1 << 7
    }
}
