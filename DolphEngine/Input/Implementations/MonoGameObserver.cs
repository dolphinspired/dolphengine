using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using DolphEngine.Input.Implementations.MgInput;

namespace DolphEngine.Input.Implementations
{
    public class MonoGameObserver : IKeyStateObserver
    {
        #region Data structures

        private long _currentGameTick;

        private bool _useKeyboard;
        private HashSet<int> _keyboardInputs;
        private Dictionary<int, Keys> _keyboardKeysByInput;
        private KeyboardState _keyboardState;

        private bool _useMouse;
        private HashSet<int> _mouseInputs;
        private MouseState _mouseState;

        private bool _useGamepads;
        private MgGamePadConfiguration _gamePadConfig;
        private HashSet<int> _gamepadInputs;
        private Dictionary<int, Buttons> _gamepadButtonsByInput;
        private Dictionary<int, GamePadState> _gamepadStatesByPlayer;

        #endregion

        #region Setup

        public void UseKeyboard()
        {
            this._keyboardInputs = GetPublicConstants(typeof(MgInput.Keyboard));
            this._keyboardKeysByInput = ((Keys[])Enum.GetValues(typeof(Keys))).ToDictionary(k => k.ToGenericInput(), k => k);
            this._useKeyboard = true;
        }

        public void UseMouse()
        {
            this._mouseInputs = GetPublicConstants(typeof(MgInput.Mouse));
            this._useMouse = true;
        }

        public void UseGamepads(int players, MgGamePadConfiguration config)
        {
            if (players < 1 || players > GamePad.MaximumGamePadCount)
            {
                throw new ArgumentException($"MonoGame only supports 1-{GamePad.MaximumGamePadCount} players!");
            }

            var gamepadInputsList = new List<int>();
            this._gamepadButtonsByInput = new Dictionary<int, Buttons>();
            this._gamepadStatesByPlayer = new Dictionary<int, GamePadState>();

            // Get Player 1's input constant that corresponds with this MonoGame Button
            // This will be used identify the button during the Update
            var recognizedButtons = (Buttons[])Enum.GetValues(typeof(Buttons));
            foreach (var button in recognizedButtons)
            {
                this._gamepadButtonsByInput.Add(button.ToGenericInput(1), button);
            }

            // Start with Player 1's Input constants as a base
            var recognizedInputs = GetPublicConstants(typeof(MgInput.GamePad.Player1));

            // For each player...
            for (var i = 1; i <= players; i++)
            {
                // Add a default gamepad state
                this._gamepadStatesByPlayer.Add(i, GamePadState.Default);

                // Add all possible unique Input keys
                gamepadInputsList.AddRange(recognizedInputs.Select(x => MgInputExtensions.GetPlayerEquivalent(x, i)));
            }

            this._gamepadInputs = gamepadInputsList.ToHashSet();
            this._useGamepads = true;
        }

        private static HashSet<int> GetPublicConstants(Type type)
        {
            var list = new HashSet<int>();

            // from: https://stackoverflow.com/a/45895466
            foreach (var field in type.GetFields())
            {
                if (field.IsLiteral && !field.IsInitOnly && field.IsPublic)
                {
                    list.Add((int)field.GetValue(null));
                }
            }

            return list;
        }

        #endregion

        #region Config

        public void SetGamePadConfiguration(MgGamePadConfiguration gamePadConfig)
        {
            this._gamePadConfig = gamePadConfig;
        }

        #endregion

        #region IKeyStateObserver implementation

        /// <inheritdoc />
        public void UpdateState(long gameTick)
        {
            this._currentGameTick = gameTick;

            if (this._useKeyboard)
            {
                this._keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            }

            if (this._useMouse)
            {
                this._mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
            }

            if (this._useGamepads)
            {
                for (var i = 1; i <= this._gamepadStatesByPlayer.Count; i++)
                {
                    this._gamepadStatesByPlayer[i] = Microsoft.Xna.Framework.Input.GamePad.GetState(i);
                }
            }
        }

        /// <inheritdoc />
        public void UpdateKey(KeyState keyState)
        {
            var key = keyState.Key;

            if (this._keyboardInputs.Contains(key))
            {
                var state = this._keyboardState;

                if (this._keyboardKeysByInput.TryGetValue(key, out var mgKey))
                {
                    // If it is a direct MonoGame key, check if it's
                    UpdateButtonOrKey(keyState, state.IsKeyDown(mgKey));
                    return;
                }
                
                // Otherwise, it must be one of the special combination inputs, such as "arrow keys"
                switch (key)
                {
                    case MgInput.Keyboard.WASD:
                        this.UpdateDirectionalPad(keyState,
                            state.IsKeyDown(Keys.W),
                            state.IsKeyDown(Keys.D),
                            state.IsKeyDown(Keys.S),
                            state.IsKeyDown(Keys.A));
                        return;
                    case MgInput.Keyboard.Arrows:
                        this.UpdateDirectionalPad(keyState,
                            state.IsKeyDown(Keys.Up),
                            state.IsKeyDown(Keys.Right),
                            state.IsKeyDown(Keys.Down),
                            state.IsKeyDown(Keys.Left));
                        return;
                    default:
                        throw new ArgumentException($"Unrecognized keyboard input: {key}!");
                }
            }

            if (this._mouseInputs.Contains(key))
            {
                var state = this._mouseState;

                switch (key)
                {
                    case MgInput.Mouse.Button1:
                        UpdateButtonOrKey(keyState, state.LeftButton == ButtonState.Pressed);
                        return;
                    case MgInput.Mouse.Button2:
                        UpdateButtonOrKey(keyState, state.RightButton == ButtonState.Pressed);
                        return;
                    case MgInput.Mouse.Button3:
                        UpdateButtonOrKey(keyState, state.MiddleButton == ButtonState.Pressed);
                        return;
                    case MgInput.Mouse.Button4:
                        UpdateButtonOrKey(keyState, state.XButton1 == ButtonState.Pressed);
                        return;
                    case MgInput.Mouse.Button5:
                        UpdateButtonOrKey(keyState, state.XButton2 == ButtonState.Pressed);
                        return;
                    case MgInput.Mouse.CursorX:
                        UpdateDigitalPosition(keyState, state.HorizontalScrollWheelValue, 0);
                        return;
                    case MgInput.Mouse.CursorY:
                        UpdateDigitalPosition(keyState, 0, state.ScrollWheelValue);
                        return;
                    case MgInput.Mouse.Cursor:
                        UpdateDigitalPosition(keyState, state.X, state.Y);
                        return;
                    case MgInput.Mouse.ScrollX:
                        UpdateDigitalPosition(keyState, state.HorizontalScrollWheelValue, 0);
                        return;
                    case MgInput.Mouse.ScrollY:
                        UpdateDigitalPosition(keyState, 0, state.ScrollWheelValue);
                        return;
                    case MgInput.Mouse.Scroll:
                        UpdateDigitalPosition(keyState, state.HorizontalScrollWheelValue, state.ScrollWheelValue);
                        return;
                    default:
                        throw new ArgumentException($"Unrecognized mouse input: {key}!");
                }
            }

            if (this._gamepadInputs.Contains(key))
            {
                var player = MgInputExtensions.GetPlayer(key);
                var state = this._gamepadStatesByPlayer[player];
                var config = this._gamePadConfig; // todo: Maybe index this by player, instead of all gamepads the same?

                // Player variable is already isolated by which state we grabbed
                // So for the sake of asking "which button was pressed?", just normalize input to Player 1's button identities
                var normalized = MgInputExtensions.GetPlayerEquivalent(key, 1);

                if (this._gamepadButtonsByInput.TryGetValue(normalized, out var mgButton))
                {
                    // If there is a corresponding MonoGame button, we can process it directly
                    switch (mgButton)
                    {
                        case Buttons.DPadUp:
                        case Buttons.DPadDown:
                        case Buttons.DPadLeft:
                        case Buttons.DPadRight:
                        case Buttons.Start:
                        case Buttons.Back:
                        case Buttons.LeftStick:
                        case Buttons.RightStick:
                        case Buttons.LeftShoulder:
                        case Buttons.RightShoulder:
                        case Buttons.BigButton:
                        case Buttons.A:
                        case Buttons.B:
                        case Buttons.X:
                        case Buttons.Y:
                            UpdateButtonOrKey(keyState, state.IsButtonDown(mgButton));
                            return;
                        case Buttons.RightTrigger:
                            UpdateAnalog(keyState, 0, state.Triggers.Right, config.TriggerThreshold, config.TriggerSensitivity);
                            return;
                        case Buttons.LeftTrigger:
                            UpdateAnalog(keyState, state.Triggers.Left, 0, config.TriggerThreshold, config.TriggerSensitivity);
                            return;
                        case Buttons.RightThumbstickUp:
                        case Buttons.RightThumbstickDown:
                            UpdateAnalog(keyState, 0, state.ThumbSticks.Right.Y, config.JoystickDeadzone, config.JoystickSensitivity);
                            return;
                        case Buttons.RightThumbstickRight:
                        case Buttons.RightThumbstickLeft:
                            UpdateAnalog(keyState, state.ThumbSticks.Right.X, 0, config.JoystickDeadzone, config.JoystickSensitivity);
                            return;
                        case Buttons.LeftThumbstickUp:
                        case Buttons.LeftThumbstickDown:
                            UpdateAnalog(keyState, 0, state.ThumbSticks.Left.Y, config.JoystickDeadzone, config.JoystickSensitivity);
                            return;
                        case Buttons.LeftThumbstickRight:
                        case Buttons.LeftThumbstickLeft:
                            UpdateAnalog(keyState, state.ThumbSticks.Left.X, 0, config.JoystickDeadzone, config.JoystickSensitivity);
                            return;
                        default:
                            throw new ArgumentException($"Unrecognized button input: {mgButton} ({(int)mgButton})!");
                    }
                }

                // Otherwise, it must be one of our special combination inputs
                // Remember that these are all normalized to Player1's constants for the sake of identity
                switch (key)
                {
                    case MgInput.GamePad.Player1.DPad:
                        this.UpdateDirectionalPad(keyState,
                            state.IsButtonDown(Buttons.DPadUp),
                            state.IsButtonDown(Buttons.DPadRight),
                            state.IsButtonDown(Buttons.DPadDown),
                            state.IsButtonDown(Buttons.DPadLeft));
                        return;
                    case MgInput.GamePad.Player1.LeftThumbstick:
                        UpdateAnalog(keyState, state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, config.JoystickDeadzone, config.JoystickSensitivity);
                        return;
                    case MgInput.GamePad.Player1.RightThumbstick:
                        UpdateAnalog(keyState, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y, config.JoystickDeadzone, config.JoystickSensitivity);
                        return;
                    case MgInput.GamePad.Player1.ABXY:
                        // todo: document somewhere that this assumes "Xbox Layout" instead of "Nintendo Layout"
                        this.UpdateDirectionalPad(keyState,
                            state.IsButtonDown(Buttons.Y),
                            state.IsButtonDown(Buttons.B),
                            state.IsButtonDown(Buttons.A),
                            state.IsButtonDown(Buttons.X));
                        return;
                    case MgInput.GamePad.Player1.Triggers:
                        UpdateAnalog(keyState, state.Triggers.Left, state.Triggers.Right, config.TriggerThreshold, config.TriggerSensitivity);
                        return;
                    default:
                        throw new ArgumentException($"Unrecognized button input: {key}!");
                }
            }
        }

        #endregion

        #region Update methods

        private void UpdateButtonOrKey(KeyState keyState, bool isPressed)
        {
            var isPressedChanged = (isPressed && !keyState.IsPressed) || (!isPressed && keyState.IsPressed);

            if (isPressedChanged)
            {
                // The button has gone from up to down, or vice versa
                keyState.IsPressed = isPressed;
                keyState.IsPressedLastChange = this._currentGameTick;

                keyState.DigitalX = isPressed ? 1 : 0;
                keyState.DigitalY = isPressed ? 1 : 0;
                keyState.DigitalLastChange = this._currentGameTick;

                keyState.AnalogX = isPressed ? 1.000f : 0;
                keyState.AnalogY = isPressed ? 1.000f : 0;
                keyState.AnalogLastChange = this._currentGameTick;
            }
        }

        private void UpdateDigital(KeyState keyState, int digitalX, int digitalY)
        {
            var isPressed = digitalX != 0 || digitalY != 0;
            var isPressedChanged = (isPressed && !keyState.IsPressed) || (!isPressed && keyState.IsPressed);

            bool isDigitalChanged = digitalX != keyState.DigitalX || digitalY != keyState.DigitalY;

            if (isPressedChanged)
            {
                // The input has changed away from or back toward (0, 0)
                keyState.IsPressed = isPressed;
                keyState.IsPressedLastChange = this._currentGameTick;
            }

            if (isDigitalChanged)
            {
                keyState.DigitalX = digitalX;
                keyState.DigitalY = digitalY;
                keyState.DigitalLastChange = this._currentGameTick;

                keyState.AnalogX = ToAnalog(digitalX);
                keyState.AnalogY = ToAnalog(digitalY);
                keyState.AnalogLastChange = this._currentGameTick;
            }
        }

        private void UpdateDigitalPosition(KeyState keyState, int digitalX, int digitalY)
        {
            bool isDigitalChanged = digitalX != keyState.DigitalX || digitalY != keyState.DigitalY;

            if (isDigitalChanged)
            {
                keyState.DigitalX = digitalX;
                keyState.DigitalY = digitalY;
                keyState.DigitalLastChange = this._currentGameTick;
            }
        }

        private void UpdateDirectionalPad(KeyState keyState, bool isPressedUp, bool isPressedRight, bool isPressedDown, bool isPressedLeft)
        {
            int digitalX = isPressedRight ? 1 : isPressedLeft ? -1 : 0;     // Up takes priority over down if both are pressed
            int digitalY = isPressedUp ? 1 : isPressedDown ? -1 : 0;        // Right takes priority over left if both are pressed
            this.UpdateDigital(keyState, digitalX, digitalY);
        }

        private void UpdateAnalog(KeyState keyState, float analogX, float analogY, float threshold, float sensitivity)
        {
            var isPressed = analogX > threshold || analogY > threshold;
            var isPressedChanged = (isPressed && !keyState.IsPressed) || (!isPressed && keyState.IsPressed);

            if (isPressedChanged)
            {
                // Stick/trigger has crossed into or out of the threshold (deadzone)
                keyState.IsPressed = isPressed;
                keyState.IsPressedLastChange = this._currentGameTick;
            }

            if (isPressed)
            {
                // Only perform the digital/analog updates if the stick/trigger is above the threshold (i.e. *not* in the deadzone)
                // Changes below the specified sensitivity will not trigger an update

                var isAnalogXChanged = Math.Abs(keyState.AnalogX - analogX) > sensitivity;
                var isAnalogYChanged = Math.Abs(keyState.AnalogY - analogY) > sensitivity;

                if (isAnalogXChanged)
                {
                    keyState.AnalogX = analogX;
                    keyState.AnalogLastChange = this._currentGameTick;

                    var digitalX = ToDigital(analogX, threshold);
                    var isDigitalXChanged = keyState.DigitalX != digitalX;

                    if (isDigitalXChanged)
                    {
                        keyState.DigitalX = digitalX;
                        keyState.DigitalLastChange = this._currentGameTick;
                    }
                }

                if (isAnalogYChanged)
                {
                    keyState.AnalogY = analogY;
                    keyState.AnalogLastChange = this._currentGameTick;

                    var digitalY = ToDigital(analogY, threshold);
                    var isDigitalYChanged = keyState.DigitalY != digitalY;

                    if (isDigitalYChanged)
                    {
                        keyState.DigitalY = digitalY;
                        keyState.DigitalLastChange = this._currentGameTick;
                    }
                }
            }
        }

        private static float ToAnalog(int digital)
        {
            if (digital == 0)
            {
                return 0;
            }
            
            return digital > 0 ? 1.000f : -1.000f;
        }

        private static int ToDigital(float analog, float threshold)
        {
            if (analog < threshold)
            {
                // If analog is below threshold (or inside deadzone), it's considered "not pressed"
                return 0;
            }

            return analog > 0 ? 1 : -1;
        }

        #endregion
    }
}
