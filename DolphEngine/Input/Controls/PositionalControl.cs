namespace DolphEngine.Input.Controls
{
    public class PositionalControl : ControlBase
    {
        public PositionalControl(string xKey, string yKey)
        {
            this.SetKeys(xKey, yKey);
        }

        public override void Update()
        {
            this.X = this.InputState.GetValueOrDefault<int>(this.Keys[0]);
            this.Y = this.InputState.GetValueOrDefault<int>(this.Keys[1]);
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}
