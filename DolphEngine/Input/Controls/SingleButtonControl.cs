namespace DolphEngine.Input.Controls
{
    public class SingleButtonControl : ControlBase
    {
        private readonly string _key;

        public SingleButtonControl(string key)
        {
            this._key = key;
            this.AddKey(key);
        }

        #region Event hooks

        public override void OnConnect()
        {
            this.LastTickPressed = this.Timer.Total.Ticks;
            this.LastTickReleased = this.Timer.Total.Ticks;
        }

        public override void OnUpdate()
        {
            var isPressed = InputState.GetValueOrDefault<bool>(this._key);

            if (isPressed && !this.IsPressed)
            {
                this.IsPressed = true;
                this.LastTickPressed = this.Timer.Total.Ticks;
            }
            else if (!isPressed && this.IsPressed)
            {
                this.IsPressed = false;
                this.LastTickReleased = this.Timer.Total.Ticks;
            }
        }

        #endregion

        public bool IsPressed { get; private set; }
        public long LastTickPressed { get; private set; }
        public long LastTickReleased { get; private set; }

        public bool JustPressed => DurationPressed == 0;
        public bool JustReleased => DurationReleased == 0;
        public long DurationPressed => !IsPressed ? -1 : this.Timer.Total.Ticks - LastTickPressed;
        public long DurationReleased => IsPressed ? -1 : this.Timer.Total.Ticks - LastTickReleased;
    }
}
