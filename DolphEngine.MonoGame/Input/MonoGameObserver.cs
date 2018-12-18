using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using DolphEngine.Input.State;
using DolphEngine.Input;

namespace DolphEngine.MonoGame.Input
{
    public class MonoGameObserver : IKeyStateObserver
    {
        #region Data structures

        protected readonly InputState InputState;

        private bool _useKeyboard;
        private KeyboardState _keyboardState;

        private bool _useMouse;
        private MouseState _mouseState;

        private bool _useGamepads;
        private Dictionary<int, GamePadState> _gamepadStatesByPlayer;

        private static readonly IReadOnlyDictionary<string, Func<KeyboardState, object>> KeyboardStateObservers = new Dictionary<string, Func<KeyboardState, object>>
        {
            #region Functions
            { InputKeys.KeyboardNone, s => s.IsKeyDown(Keys.None) },
            { InputKeys.KeyboardBack, s => s.IsKeyDown(Keys.Back) },
            { InputKeys.KeyboardTab, s => s.IsKeyDown(Keys.Tab) },
            { InputKeys.KeyboardEnter, s => s.IsKeyDown(Keys.Enter) },
            { InputKeys.KeyboardPause, s => s.IsKeyDown(Keys.Pause) },
            { InputKeys.KeyboardCapsLock, s => s.IsKeyDown(Keys.CapsLock) },
            { InputKeys.KeyboardKana, s => s.IsKeyDown(Keys.Kana) },
            { InputKeys.KeyboardKanji, s => s.IsKeyDown(Keys.Kanji) },
            { InputKeys.KeyboardEscape, s => s.IsKeyDown(Keys.Escape) },
            { InputKeys.KeyboardImeConvert, s => s.IsKeyDown(Keys.ImeConvert) },
            { InputKeys.KeyboardImeNoConvert, s => s.IsKeyDown(Keys.ImeNoConvert) },
            { InputKeys.KeyboardSpace, s => s.IsKeyDown(Keys.Space) },
            { InputKeys.KeyboardPageUp, s => s.IsKeyDown(Keys.PageUp) },
            { InputKeys.KeyboardPageDown, s => s.IsKeyDown(Keys.PageDown) },
            { InputKeys.KeyboardEnd, s => s.IsKeyDown(Keys.End) },
            { InputKeys.KeyboardHome, s => s.IsKeyDown(Keys.Home) },
            { InputKeys.KeyboardLeft, s => s.IsKeyDown(Keys.Left) },
            { InputKeys.KeyboardUp, s => s.IsKeyDown(Keys.Up) },
            { InputKeys.KeyboardRight, s => s.IsKeyDown(Keys.Right) },
            { InputKeys.KeyboardDown, s => s.IsKeyDown(Keys.Down) },
            { InputKeys.KeyboardSelect, s => s.IsKeyDown(Keys.Select) },
            { InputKeys.KeyboardPrint, s => s.IsKeyDown(Keys.Print) },
            { InputKeys.KeyboardExecute, s => s.IsKeyDown(Keys.Execute) },
            { InputKeys.KeyboardPrintScreen, s => s.IsKeyDown(Keys.PrintScreen) },
            { InputKeys.KeyboardInsert, s => s.IsKeyDown(Keys.Insert) },
            { InputKeys.KeyboardDelete, s => s.IsKeyDown(Keys.Delete) },
            { InputKeys.KeyboardHelp, s => s.IsKeyDown(Keys.Help) },
            { InputKeys.KeyboardD0, s => s.IsKeyDown(Keys.D0) },
            { InputKeys.KeyboardD1, s => s.IsKeyDown(Keys.D1) },
            { InputKeys.KeyboardD2, s => s.IsKeyDown(Keys.D2) },
            { InputKeys.KeyboardD3, s => s.IsKeyDown(Keys.D3) },
            { InputKeys.KeyboardD4, s => s.IsKeyDown(Keys.D4) },
            { InputKeys.KeyboardD5, s => s.IsKeyDown(Keys.D5) },
            { InputKeys.KeyboardD6, s => s.IsKeyDown(Keys.D6) },
            { InputKeys.KeyboardD7, s => s.IsKeyDown(Keys.D7) },
            { InputKeys.KeyboardD8, s => s.IsKeyDown(Keys.D8) },
            { InputKeys.KeyboardD9, s => s.IsKeyDown(Keys.D9) },
            { InputKeys.KeyboardA, s => s.IsKeyDown(Keys.A) },
            { InputKeys.KeyboardB, s => s.IsKeyDown(Keys.B) },
            { InputKeys.KeyboardC, s => s.IsKeyDown(Keys.C) },
            { InputKeys.KeyboardD, s => s.IsKeyDown(Keys.D) },
            { InputKeys.KeyboardE, s => s.IsKeyDown(Keys.E) },
            { InputKeys.KeyboardF, s => s.IsKeyDown(Keys.F) },
            { InputKeys.KeyboardG, s => s.IsKeyDown(Keys.G) },
            { InputKeys.KeyboardH, s => s.IsKeyDown(Keys.H) },
            { InputKeys.KeyboardI, s => s.IsKeyDown(Keys.I) },
            { InputKeys.KeyboardJ, s => s.IsKeyDown(Keys.J) },
            { InputKeys.KeyboardK, s => s.IsKeyDown(Keys.K) },
            { InputKeys.KeyboardL, s => s.IsKeyDown(Keys.L) },
            { InputKeys.KeyboardM, s => s.IsKeyDown(Keys.M) },
            { InputKeys.KeyboardN, s => s.IsKeyDown(Keys.N) },
            { InputKeys.KeyboardO, s => s.IsKeyDown(Keys.O) },
            { InputKeys.KeyboardP, s => s.IsKeyDown(Keys.P) },
            { InputKeys.KeyboardQ, s => s.IsKeyDown(Keys.Q) },
            { InputKeys.KeyboardR, s => s.IsKeyDown(Keys.R) },
            { InputKeys.KeyboardS, s => s.IsKeyDown(Keys.S) },
            { InputKeys.KeyboardT, s => s.IsKeyDown(Keys.T) },
            { InputKeys.KeyboardU, s => s.IsKeyDown(Keys.U) },
            { InputKeys.KeyboardV, s => s.IsKeyDown(Keys.V) },
            { InputKeys.KeyboardW, s => s.IsKeyDown(Keys.W) },
            { InputKeys.KeyboardX, s => s.IsKeyDown(Keys.X) },
            { InputKeys.KeyboardY, s => s.IsKeyDown(Keys.Y) },
            { InputKeys.KeyboardZ, s => s.IsKeyDown(Keys.Z) },
            { InputKeys.KeyboardLeftWindows, s => s.IsKeyDown(Keys.LeftWindows) },
            { InputKeys.KeyboardRightWindows, s => s.IsKeyDown(Keys.RightWindows) },
            { InputKeys.KeyboardApps, s => s.IsKeyDown(Keys.Apps) },
            { InputKeys.KeyboardSleep, s => s.IsKeyDown(Keys.Sleep) },
            { InputKeys.KeyboardNumPad0, s => s.IsKeyDown(Keys.NumPad0) },
            { InputKeys.KeyboardNumPad1, s => s.IsKeyDown(Keys.NumPad1) },
            { InputKeys.KeyboardNumPad2, s => s.IsKeyDown(Keys.NumPad2) },
            { InputKeys.KeyboardNumPad3, s => s.IsKeyDown(Keys.NumPad3) },
            { InputKeys.KeyboardNumPad4, s => s.IsKeyDown(Keys.NumPad4) },
            { InputKeys.KeyboardNumPad5, s => s.IsKeyDown(Keys.NumPad5) },
            { InputKeys.KeyboardNumPad6, s => s.IsKeyDown(Keys.NumPad6) },
            { InputKeys.KeyboardNumPad7, s => s.IsKeyDown(Keys.NumPad7) },
            { InputKeys.KeyboardNumPad8, s => s.IsKeyDown(Keys.NumPad8) },
            { InputKeys.KeyboardNumPad9, s => s.IsKeyDown(Keys.NumPad9) },
            { InputKeys.KeyboardMultiply, s => s.IsKeyDown(Keys.Multiply) },
            { InputKeys.KeyboardAdd, s => s.IsKeyDown(Keys.Add) },
            { InputKeys.KeyboardSeparator, s => s.IsKeyDown(Keys.Separator) },
            { InputKeys.KeyboardSubtract, s => s.IsKeyDown(Keys.Subtract) },
            { InputKeys.KeyboardDecimal, s => s.IsKeyDown(Keys.Decimal) },
            { InputKeys.KeyboardDivide, s => s.IsKeyDown(Keys.Divide) },
            { InputKeys.KeyboardF1, s => s.IsKeyDown(Keys.F1) },
            { InputKeys.KeyboardF2, s => s.IsKeyDown(Keys.F2) },
            { InputKeys.KeyboardF3, s => s.IsKeyDown(Keys.F3) },
            { InputKeys.KeyboardF4, s => s.IsKeyDown(Keys.F4) },
            { InputKeys.KeyboardF5, s => s.IsKeyDown(Keys.F5) },
            { InputKeys.KeyboardF6, s => s.IsKeyDown(Keys.F6) },
            { InputKeys.KeyboardF7, s => s.IsKeyDown(Keys.F7) },
            { InputKeys.KeyboardF8, s => s.IsKeyDown(Keys.F8) },
            { InputKeys.KeyboardF9, s => s.IsKeyDown(Keys.F9) },
            { InputKeys.KeyboardF10, s => s.IsKeyDown(Keys.F10) },
            { InputKeys.KeyboardF11, s => s.IsKeyDown(Keys.F11) },
            { InputKeys.KeyboardF12, s => s.IsKeyDown(Keys.F12) },
            { InputKeys.KeyboardF13, s => s.IsKeyDown(Keys.F13) },
            { InputKeys.KeyboardF14, s => s.IsKeyDown(Keys.F14) },
            { InputKeys.KeyboardF15, s => s.IsKeyDown(Keys.F15) },
            { InputKeys.KeyboardF16, s => s.IsKeyDown(Keys.F16) },
            { InputKeys.KeyboardF17, s => s.IsKeyDown(Keys.F17) },
            { InputKeys.KeyboardF18, s => s.IsKeyDown(Keys.F18) },
            { InputKeys.KeyboardF19, s => s.IsKeyDown(Keys.F19) },
            { InputKeys.KeyboardF20, s => s.IsKeyDown(Keys.F20) },
            { InputKeys.KeyboardF21, s => s.IsKeyDown(Keys.F21) },
            { InputKeys.KeyboardF22, s => s.IsKeyDown(Keys.F22) },
            { InputKeys.KeyboardF23, s => s.IsKeyDown(Keys.F23) },
            { InputKeys.KeyboardF24, s => s.IsKeyDown(Keys.F24) },
            { InputKeys.KeyboardNumLock, s => s.IsKeyDown(Keys.NumLock) },
            { InputKeys.KeyboardScroll, s => s.IsKeyDown(Keys.Scroll) },
            { InputKeys.KeyboardLeftShift, s => s.IsKeyDown(Keys.LeftShift) },
            { InputKeys.KeyboardRightShift, s => s.IsKeyDown(Keys.RightShift) },
            { InputKeys.KeyboardLeftControl, s => s.IsKeyDown(Keys.LeftControl) },
            { InputKeys.KeyboardRightControl, s => s.IsKeyDown(Keys.RightControl) },
            { InputKeys.KeyboardLeftAlt, s => s.IsKeyDown(Keys.LeftAlt) },
            { InputKeys.KeyboardRightAlt, s => s.IsKeyDown(Keys.RightAlt) },
            { InputKeys.KeyboardBrowserBack, s => s.IsKeyDown(Keys.BrowserBack) },
            { InputKeys.KeyboardBrowserForward, s => s.IsKeyDown(Keys.BrowserForward) },
            { InputKeys.KeyboardBrowserRefresh, s => s.IsKeyDown(Keys.BrowserRefresh) },
            { InputKeys.KeyboardBrowserStop, s => s.IsKeyDown(Keys.BrowserStop) },
            { InputKeys.KeyboardBrowserSearch, s => s.IsKeyDown(Keys.BrowserSearch) },
            { InputKeys.KeyboardBrowserFavorites, s => s.IsKeyDown(Keys.BrowserFavorites) },
            { InputKeys.KeyboardBrowserHome, s => s.IsKeyDown(Keys.BrowserHome) },
            { InputKeys.KeyboardVolumeMute, s => s.IsKeyDown(Keys.VolumeMute) },
            { InputKeys.KeyboardVolumeDown, s => s.IsKeyDown(Keys.VolumeDown) },
            { InputKeys.KeyboardVolumeUp, s => s.IsKeyDown(Keys.VolumeUp) },
            { InputKeys.KeyboardMediaNextTrack, s => s.IsKeyDown(Keys.MediaNextTrack) },
            { InputKeys.KeyboardMediaPreviousTrack, s => s.IsKeyDown(Keys.MediaPreviousTrack) },
            { InputKeys.KeyboardMediaStop, s => s.IsKeyDown(Keys.MediaStop) },
            { InputKeys.KeyboardMediaPlayPause, s => s.IsKeyDown(Keys.MediaPlayPause) },
            { InputKeys.KeyboardLaunchMail, s => s.IsKeyDown(Keys.LaunchMail) },
            { InputKeys.KeyboardSelectMedia, s => s.IsKeyDown(Keys.SelectMedia) },
            { InputKeys.KeyboardLaunchApplication1, s => s.IsKeyDown(Keys.LaunchApplication1) },
            { InputKeys.KeyboardLaunchApplication2, s => s.IsKeyDown(Keys.LaunchApplication2) },
            { InputKeys.KeyboardOemSemicolon, s => s.IsKeyDown(Keys.OemSemicolon) },
            { InputKeys.KeyboardOemPlus, s => s.IsKeyDown(Keys.OemPlus) },
            { InputKeys.KeyboardOemComma, s => s.IsKeyDown(Keys.OemComma) },
            { InputKeys.KeyboardOemMinus, s => s.IsKeyDown(Keys.OemMinus) },
            { InputKeys.KeyboardOemPeriod, s => s.IsKeyDown(Keys.OemPeriod) },
            { InputKeys.KeyboardOemQuestion, s => s.IsKeyDown(Keys.OemQuestion) },
            { InputKeys.KeyboardOemTilde, s => s.IsKeyDown(Keys.OemTilde) },
            { InputKeys.KeyboardChatPadGreen, s => s.IsKeyDown(Keys.ChatPadGreen) },
            { InputKeys.KeyboardChatPadOrange, s => s.IsKeyDown(Keys.ChatPadOrange) },
            { InputKeys.KeyboardOemOpenBrackets, s => s.IsKeyDown(Keys.OemOpenBrackets) },
            { InputKeys.KeyboardOemPipe, s => s.IsKeyDown(Keys.OemPipe) },
            { InputKeys.KeyboardOemCloseBrackets, s => s.IsKeyDown(Keys.OemCloseBrackets) },
            { InputKeys.KeyboardOemQuotes, s => s.IsKeyDown(Keys.OemQuotes) },
            { InputKeys.KeyboardOem8, s => s.IsKeyDown(Keys.Oem8) },
            { InputKeys.KeyboardOemBackslash, s => s.IsKeyDown(Keys.OemBackslash) },
            { InputKeys.KeyboardProcessKey, s => s.IsKeyDown(Keys.ProcessKey) },
            { InputKeys.KeyboardOemCopy, s => s.IsKeyDown(Keys.OemCopy) },
            { InputKeys.KeyboardOemAuto, s => s.IsKeyDown(Keys.OemAuto) },
            { InputKeys.KeyboardOemEnlW, s => s.IsKeyDown(Keys.OemEnlW) },
            { InputKeys.KeyboardAttn, s => s.IsKeyDown(Keys.Attn) },
            { InputKeys.KeyboardCrsel, s => s.IsKeyDown(Keys.Crsel) },
            { InputKeys.KeyboardExsel, s => s.IsKeyDown(Keys.Exsel) },
            { InputKeys.KeyboardEraseEof, s => s.IsKeyDown(Keys.EraseEof) },
            { InputKeys.KeyboardPlay, s => s.IsKeyDown(Keys.Play) },
            { InputKeys.KeyboardZoom, s => s.IsKeyDown(Keys.Zoom) },
            { InputKeys.KeyboardPa1, s => s.IsKeyDown(Keys.Pa1) },
            { InputKeys.KeyboardOemClear, s => s.IsKeyDown(Keys.OemClear) },
            #endregion
        };

        private static readonly IReadOnlyDictionary<string, Func<MouseState, object>> MouseStateObservers = new Dictionary<string, Func<MouseState, object>>
        {
            #region Functions
            { InputKeys.MouseButton1, s => s.LeftButton == ButtonState.Pressed },
            { InputKeys.MouseButton2, s => s.RightButton == ButtonState.Pressed },
            { InputKeys.MouseButton3, s => s.MiddleButton == ButtonState.Pressed },
            { InputKeys.MouseButton4, s => s.XButton1 == ButtonState.Pressed },
            { InputKeys.MouseButton5, s => s.XButton2 == ButtonState.Pressed },
            { InputKeys.MouseCursorX, s => s.Position.X },
            { InputKeys.MouseCursorY, s => s.Position.Y },
            { InputKeys.MouseScrollX, s => s.HorizontalScrollWheelValue },
            { InputKeys.MouseScrollY, s => s.ScrollWheelValue },
            #endregion
        };

        private static readonly IReadOnlyDictionary<string, Func<GamePadState, object>> GamePadStateObservers = new Dictionary<string, Func<GamePadState, object>>
        {
            #region Functions
            { InputKeys.GamepadDPadUp, s => s.IsButtonDown(Buttons.DPadUp) },
            { InputKeys.GamepadDPadDown, s => s.IsButtonDown(Buttons.DPadDown) },
            { InputKeys.GamepadDPadLeft, s => s.IsButtonDown(Buttons.DPadLeft) },
            { InputKeys.GamepadDPadRight, s => s.IsButtonDown(Buttons.DPadRight) },
            { InputKeys.GamepadStart, s => s.IsButtonDown(Buttons.Start) },
            { InputKeys.GamepadBack, s => s.IsButtonDown(Buttons.Back) },
            { InputKeys.GamepadLeftStick, s => s.IsButtonDown(Buttons.LeftStick) },
            { InputKeys.GamepadRightStick, s => s.IsButtonDown(Buttons.RightStick) },
            { InputKeys.GamepadLeftShoulder, s => s.IsButtonDown(Buttons.LeftShoulder) },
            { InputKeys.GamepadRightShoulder, s => s.IsButtonDown(Buttons.RightShoulder) },
            { InputKeys.GamepadBigButton, s => s.IsButtonDown(Buttons.BigButton) },
            { InputKeys.GamepadA, s => s.IsButtonDown(Buttons.A) },
            { InputKeys.GamepadB, s => s.IsButtonDown(Buttons.B) },
            { InputKeys.GamepadX, s => s.IsButtonDown(Buttons.X) },
            { InputKeys.GamepadY, s => s.IsButtonDown(Buttons.Y) },
            { InputKeys.GamepadRightTrigger, s => s.Triggers.Right },
            { InputKeys.GamepadLeftTrigger, s => s.Triggers.Left },
            { InputKeys.GamepadRightThumbstickX, s => s.ThumbSticks.Right.X },
            { InputKeys.GamepadRightThumbstickY, s => s.ThumbSticks.Right.Y },
            { InputKeys.GamepadLeftThumbstickX, s => s.ThumbSticks.Left.X },
            { InputKeys.GamepadLeftThumbstickY, s => s.ThumbSticks.Right.Y },
            #endregion
        };

        #endregion

        #region Setup

        public MonoGameObserver UseKeyboard()
        {
            this._useKeyboard = true;
            return this;
        }

        public MonoGameObserver UseMouse()
        {
            this._useMouse = true;
            return this;
        }

        public MonoGameObserver UseGamepads(int players)
        {
            if (players < 1 || players > GamePad.MaximumGamePadCount)
            {
                throw new ArgumentException($"MonoGame only supports 1-{GamePad.MaximumGamePadCount} players!");
            }

            this._gamepadStatesByPlayer = new Dictionary<int, GamePadState>();

            // For each player...
            for (var i = 1; i <= players; i++)
            {
                // Add a default gamepad state
                this._gamepadStatesByPlayer.Add(i, GamePadState.Default);
            }

            this._useGamepads = true;
            return this;
        }

        #endregion

        #region IKeyStateObserver implementation

        /// <inheritdoc />
        public void UpdateState()
        {
            if (this._useKeyboard)
            {
                this._keyboardState = Keyboard.GetState();
            }

            if (this._useMouse)
            {
                this._mouseState = Mouse.GetState();
            }

            if (this._useGamepads)
            {
                for (var i = 1; i <= this._gamepadStatesByPlayer.Count; i++)
                {
                    this._gamepadStatesByPlayer[i] = GamePad.GetState(i);
                }
            }
        }

        /// <inheritdoc />
        public object GetKeyValue(InputKey key)
        {
            var gkey = key.GenericKey;

            if (KeyboardStateObservers.TryGetValue(gkey, out var kreader))
            {
                return kreader(this._keyboardState);
            }
            else if (MouseStateObservers.TryGetValue(gkey, out var mreader))
            {
                return mreader(this._mouseState);
            }
            else if (GamePadStateObservers.TryGetValue(gkey, out var greader))
            {
                return greader(this._gamepadStatesByPlayer[key.Player]);
            }
            else
            {
                throw new ArgumentException($"Unrecognized generic input key ({gkey})!");
            }
        }

        #endregion
    }
}