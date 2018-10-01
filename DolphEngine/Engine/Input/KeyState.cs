namespace TacticsGame.Engine.Input
{
    public class KeyState : IReadOnlyKeyState
    {
        public KeyState(int key)
        {
            this.Key = key;
        }

        public int Key { get; }

        public bool IsPressed { get; set; }

        public long IsPressedLastChange { get; set; }

        public int DigitalX { get; set; }

        public int DigitalY { get; set; }

        public long DigitalLastChange { get; set; }

        public float AnalogX { get; set; }

        public float AnalogY { get; set; }

        public long AnalogLastChange { get; set; }

        #region Object overrides

        public override string ToString()
        {
            return $"{{Key:{this.Key}}}";
        }

        #endregion
    }

    public interface IReadOnlyKeyState
    {
        int Key { get; }

        bool IsPressed { get; }

        long IsPressedLastChange { get; }

        int DigitalX { get; }

        int DigitalY { get; }

        long DigitalLastChange { get; }

        float AnalogX { get; }

        float AnalogY { get; }

        long AnalogLastChange { get; }
    }
}
