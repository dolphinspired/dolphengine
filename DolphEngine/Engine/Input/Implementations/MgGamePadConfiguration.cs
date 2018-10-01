namespace TacticsGame.Engine.Input.Implementations
{
    public struct MgGamePadConfiguration
    {
        public static MgGamePadConfiguration Default = new MgGamePadConfiguration
        {
            JoystickDeadzone = 0.250f,
            JoystickSensitivity = 0.010f,
            TriggerThreshold = 0.500f,
            TriggerSensitivity = 0.010f
        };

        public float JoystickDeadzone;
        public float JoystickSensitivity;
        public float TriggerThreshold;
        public float TriggerSensitivity;
    }
}
