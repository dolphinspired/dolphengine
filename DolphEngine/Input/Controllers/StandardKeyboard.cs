using DolphEngine.Input.Controls;
using DolphEngine.Input.State;
using System.Collections.Generic;

namespace DolphEngine.Input.Controllers
{
    public class StandardKeyboard : ControlBase
    {
        private readonly Dictionary<string, SingleButtonControl> _keyControls = new Dictionary<string, SingleButtonControl>();

        public StandardKeyboard()
        {
            this.SetKeys(
                #region Keys
                InputKeys.KeyboardNone,
                InputKeys.KeyboardBack,
                InputKeys.KeyboardTab,
                InputKeys.KeyboardEnter,
                InputKeys.KeyboardPause,
                InputKeys.KeyboardCapsLock,
                InputKeys.KeyboardKana,
                InputKeys.KeyboardKanji,
                InputKeys.KeyboardEscape,
                InputKeys.KeyboardImeConvert,
                InputKeys.KeyboardImeNoConvert,
                InputKeys.KeyboardSpace,
                InputKeys.KeyboardPageUp,
                InputKeys.KeyboardPageDown,
                InputKeys.KeyboardEnd,
                InputKeys.KeyboardHome,
                InputKeys.KeyboardLeft,
                InputKeys.KeyboardUp,
                InputKeys.KeyboardRight,
                InputKeys.KeyboardDown,
                InputKeys.KeyboardSelect,
                InputKeys.KeyboardPrint,
                InputKeys.KeyboardExecute,
                InputKeys.KeyboardPrintScreen,
                InputKeys.KeyboardInsert,
                InputKeys.KeyboardDelete,
                InputKeys.KeyboardHelp,
                InputKeys.KeyboardD0,
                InputKeys.KeyboardD1,
                InputKeys.KeyboardD2,
                InputKeys.KeyboardD3,
                InputKeys.KeyboardD4,
                InputKeys.KeyboardD5,
                InputKeys.KeyboardD6,
                InputKeys.KeyboardD7,
                InputKeys.KeyboardD8,
                InputKeys.KeyboardD9,
                InputKeys.KeyboardA,
                InputKeys.KeyboardB,
                InputKeys.KeyboardC,
                InputKeys.KeyboardD,
                InputKeys.KeyboardE,
                InputKeys.KeyboardF,
                InputKeys.KeyboardG,
                InputKeys.KeyboardH,
                InputKeys.KeyboardI,
                InputKeys.KeyboardJ,
                InputKeys.KeyboardK,
                InputKeys.KeyboardL,
                InputKeys.KeyboardM,
                InputKeys.KeyboardN,
                InputKeys.KeyboardO,
                InputKeys.KeyboardP,
                InputKeys.KeyboardQ,
                InputKeys.KeyboardR,
                InputKeys.KeyboardS,
                InputKeys.KeyboardT,
                InputKeys.KeyboardU,
                InputKeys.KeyboardV,
                InputKeys.KeyboardW,
                InputKeys.KeyboardX,
                InputKeys.KeyboardY,
                InputKeys.KeyboardZ,
                InputKeys.KeyboardLeftWindows,
                InputKeys.KeyboardRightWindows,
                InputKeys.KeyboardApps,
                InputKeys.KeyboardSleep,
                InputKeys.KeyboardNumPad0,
                InputKeys.KeyboardNumPad1,
                InputKeys.KeyboardNumPad2,
                InputKeys.KeyboardNumPad3,
                InputKeys.KeyboardNumPad4,
                InputKeys.KeyboardNumPad5,
                InputKeys.KeyboardNumPad6,
                InputKeys.KeyboardNumPad7,
                InputKeys.KeyboardNumPad8,
                InputKeys.KeyboardNumPad9,
                InputKeys.KeyboardMultiply,
                InputKeys.KeyboardAdd,
                InputKeys.KeyboardSeparator,
                InputKeys.KeyboardSubtract,
                InputKeys.KeyboardDecimal,
                InputKeys.KeyboardDivide,
                InputKeys.KeyboardF1,
                InputKeys.KeyboardF2,
                InputKeys.KeyboardF3,
                InputKeys.KeyboardF4,
                InputKeys.KeyboardF5,
                InputKeys.KeyboardF6,
                InputKeys.KeyboardF7,
                InputKeys.KeyboardF8,
                InputKeys.KeyboardF9,
                InputKeys.KeyboardF10,
                InputKeys.KeyboardF11,
                InputKeys.KeyboardF12,
                InputKeys.KeyboardF13,
                InputKeys.KeyboardF14,
                InputKeys.KeyboardF15,
                InputKeys.KeyboardF16,
                InputKeys.KeyboardF17,
                InputKeys.KeyboardF18,
                InputKeys.KeyboardF19,
                InputKeys.KeyboardF20,
                InputKeys.KeyboardF21,
                InputKeys.KeyboardF22,
                InputKeys.KeyboardF23,
                InputKeys.KeyboardF24,
                InputKeys.KeyboardNumLock,
                InputKeys.KeyboardScroll,
                InputKeys.KeyboardLeftShift,
                InputKeys.KeyboardRightShift,
                InputKeys.KeyboardLeftControl,
                InputKeys.KeyboardRightControl,
                InputKeys.KeyboardLeftAlt,
                InputKeys.KeyboardRightAlt,
                InputKeys.KeyboardBrowserBack,
                InputKeys.KeyboardBrowserForward,
                InputKeys.KeyboardBrowserRefresh,
                InputKeys.KeyboardBrowserStop,
                InputKeys.KeyboardBrowserSearch,
                InputKeys.KeyboardBrowserFavorites,
                InputKeys.KeyboardBrowserHome,
                InputKeys.KeyboardVolumeMute,
                InputKeys.KeyboardVolumeDown,
                InputKeys.KeyboardVolumeUp,
                InputKeys.KeyboardMediaNextTrack,
                InputKeys.KeyboardMediaPreviousTrack,
                InputKeys.KeyboardMediaStop,
                InputKeys.KeyboardMediaPlayPause,
                InputKeys.KeyboardLaunchMail,
                InputKeys.KeyboardSelectMedia,
                InputKeys.KeyboardLaunchApplication1,
                InputKeys.KeyboardLaunchApplication2,
                InputKeys.KeyboardOemSemicolon,
                InputKeys.KeyboardOemPlus,
                InputKeys.KeyboardOemComma,
                InputKeys.KeyboardOemMinus,
                InputKeys.KeyboardOemPeriod,
                InputKeys.KeyboardOemQuestion,
                InputKeys.KeyboardOemTilde,
                InputKeys.KeyboardChatPadGreen,
                InputKeys.KeyboardChatPadOrange,
                InputKeys.KeyboardOemOpenBrackets,
                InputKeys.KeyboardOemPipe,
                InputKeys.KeyboardOemCloseBrackets,
                InputKeys.KeyboardOemQuotes,
                InputKeys.KeyboardOem8,
                InputKeys.KeyboardOemBackslash,
                InputKeys.KeyboardProcessKey,
                InputKeys.KeyboardOemCopy,
                InputKeys.KeyboardOemAuto,
                InputKeys.KeyboardOemEnlW,
                InputKeys.KeyboardAttn,
                InputKeys.KeyboardCrsel,
                InputKeys.KeyboardExsel,
                InputKeys.KeyboardEraseEof,
                InputKeys.KeyboardPlay,
                InputKeys.KeyboardZoom,
                InputKeys.KeyboardPa1,
                InputKeys.KeyboardOemClear
                #endregion
            );

            foreach (var key in this.Keys)
            {
                this._keyControls.Add(key, new SingleButtonControl(key));
            }

            this.ArrowKeys = new DirectionalPadControl(InputKeys.KeyboardUp, InputKeys.KeyboardRight, InputKeys.KeyboardDown, InputKeys.KeyboardLeft);
            this.WASD = new DirectionalPadControl(InputKeys.KeyboardW, InputKeys.KeyboardD, InputKeys.KeyboardS, InputKeys.KeyboardA);
        }

        public override void SetInputState(InputState inputState)
        {
            base.SetInputState(inputState);

            foreach (var control in this._keyControls)
            {
                control.Value.SetInputState(inputState);
            }

            this.ArrowKeys.SetInputState(inputState);
            this.WASD.SetInputState(inputState);
        }

        public override void Update()
        {
            foreach (var control in this._keyControls)
            {
                control.Value.Update();
            }

            this.ArrowKeys.Update();
            this.WASD.Update();
        }

        #region Key control getters
        public SingleButtonControl None => this._keyControls[InputKeys.KeyboardNone];
        public SingleButtonControl Back => this._keyControls[InputKeys.KeyboardBack];
        public SingleButtonControl Tab => this._keyControls[InputKeys.KeyboardTab];
        public SingleButtonControl Enter => this._keyControls[InputKeys.KeyboardEnter];
        public SingleButtonControl Pause => this._keyControls[InputKeys.KeyboardPause];
        public SingleButtonControl CapsLock => this._keyControls[InputKeys.KeyboardCapsLock];
        public SingleButtonControl Kana => this._keyControls[InputKeys.KeyboardKana];
        public SingleButtonControl Kanji => this._keyControls[InputKeys.KeyboardKanji];
        public SingleButtonControl Escape => this._keyControls[InputKeys.KeyboardEscape];
        public SingleButtonControl ImeConvert => this._keyControls[InputKeys.KeyboardImeConvert];
        public SingleButtonControl ImeNoConvert => this._keyControls[InputKeys.KeyboardImeNoConvert];
        public SingleButtonControl Space => this._keyControls[InputKeys.KeyboardSpace];
        public SingleButtonControl PageUp => this._keyControls[InputKeys.KeyboardPageUp];
        public SingleButtonControl PageDown => this._keyControls[InputKeys.KeyboardPageDown];
        public SingleButtonControl End => this._keyControls[InputKeys.KeyboardEnd];
        public SingleButtonControl Home => this._keyControls[InputKeys.KeyboardHome];
        public SingleButtonControl Left => this._keyControls[InputKeys.KeyboardLeft];
        public SingleButtonControl Up => this._keyControls[InputKeys.KeyboardUp];
        public SingleButtonControl Right => this._keyControls[InputKeys.KeyboardRight];
        public SingleButtonControl Down => this._keyControls[InputKeys.KeyboardDown];
        public SingleButtonControl Select => this._keyControls[InputKeys.KeyboardSelect];
        public SingleButtonControl Print => this._keyControls[InputKeys.KeyboardPrint];
        public SingleButtonControl Execute => this._keyControls[InputKeys.KeyboardExecute];
        public SingleButtonControl PrintScreen => this._keyControls[InputKeys.KeyboardPrintScreen];
        public SingleButtonControl Insert => this._keyControls[InputKeys.KeyboardInsert];
        public SingleButtonControl Delete => this._keyControls[InputKeys.KeyboardDelete];
        public SingleButtonControl Help => this._keyControls[InputKeys.KeyboardHelp];
        public SingleButtonControl D0 => this._keyControls[InputKeys.KeyboardD0];
        public SingleButtonControl D1 => this._keyControls[InputKeys.KeyboardD1];
        public SingleButtonControl D2 => this._keyControls[InputKeys.KeyboardD2];
        public SingleButtonControl D3 => this._keyControls[InputKeys.KeyboardD3];
        public SingleButtonControl D4 => this._keyControls[InputKeys.KeyboardD4];
        public SingleButtonControl D5 => this._keyControls[InputKeys.KeyboardD5];
        public SingleButtonControl D6 => this._keyControls[InputKeys.KeyboardD6];
        public SingleButtonControl D7 => this._keyControls[InputKeys.KeyboardD7];
        public SingleButtonControl D8 => this._keyControls[InputKeys.KeyboardD8];
        public SingleButtonControl D9 => this._keyControls[InputKeys.KeyboardD9];
        public SingleButtonControl A => this._keyControls[InputKeys.KeyboardA];
        public SingleButtonControl B => this._keyControls[InputKeys.KeyboardB];
        public SingleButtonControl C => this._keyControls[InputKeys.KeyboardC];
        public SingleButtonControl D => this._keyControls[InputKeys.KeyboardD];
        public SingleButtonControl E => this._keyControls[InputKeys.KeyboardE];
        public SingleButtonControl F => this._keyControls[InputKeys.KeyboardF];
        public SingleButtonControl G => this._keyControls[InputKeys.KeyboardG];
        public SingleButtonControl H => this._keyControls[InputKeys.KeyboardH];
        public SingleButtonControl I => this._keyControls[InputKeys.KeyboardI];
        public SingleButtonControl J => this._keyControls[InputKeys.KeyboardJ];
        public SingleButtonControl K => this._keyControls[InputKeys.KeyboardK];
        public SingleButtonControl L => this._keyControls[InputKeys.KeyboardL];
        public SingleButtonControl M => this._keyControls[InputKeys.KeyboardM];
        public SingleButtonControl N => this._keyControls[InputKeys.KeyboardN];
        public SingleButtonControl O => this._keyControls[InputKeys.KeyboardO];
        public SingleButtonControl P => this._keyControls[InputKeys.KeyboardP];
        public SingleButtonControl Q => this._keyControls[InputKeys.KeyboardQ];
        public SingleButtonControl R => this._keyControls[InputKeys.KeyboardR];
        public SingleButtonControl S => this._keyControls[InputKeys.KeyboardS];
        public SingleButtonControl T => this._keyControls[InputKeys.KeyboardT];
        public SingleButtonControl U => this._keyControls[InputKeys.KeyboardU];
        public SingleButtonControl V => this._keyControls[InputKeys.KeyboardV];
        public SingleButtonControl W => this._keyControls[InputKeys.KeyboardW];
        public SingleButtonControl X => this._keyControls[InputKeys.KeyboardX];
        public SingleButtonControl Y => this._keyControls[InputKeys.KeyboardY];
        public SingleButtonControl Z => this._keyControls[InputKeys.KeyboardZ];
        public SingleButtonControl LeftWindows => this._keyControls[InputKeys.KeyboardLeftWindows];
        public SingleButtonControl RightWindows => this._keyControls[InputKeys.KeyboardRightWindows];
        public SingleButtonControl Apps => this._keyControls[InputKeys.KeyboardApps];
        public SingleButtonControl Sleep => this._keyControls[InputKeys.KeyboardSleep];
        public SingleButtonControl NumPad0 => this._keyControls[InputKeys.KeyboardNumPad0];
        public SingleButtonControl NumPad1 => this._keyControls[InputKeys.KeyboardNumPad1];
        public SingleButtonControl NumPad2 => this._keyControls[InputKeys.KeyboardNumPad2];
        public SingleButtonControl NumPad3 => this._keyControls[InputKeys.KeyboardNumPad3];
        public SingleButtonControl NumPad4 => this._keyControls[InputKeys.KeyboardNumPad4];
        public SingleButtonControl NumPad5 => this._keyControls[InputKeys.KeyboardNumPad5];
        public SingleButtonControl NumPad6 => this._keyControls[InputKeys.KeyboardNumPad6];
        public SingleButtonControl NumPad7 => this._keyControls[InputKeys.KeyboardNumPad7];
        public SingleButtonControl NumPad8 => this._keyControls[InputKeys.KeyboardNumPad8];
        public SingleButtonControl NumPad9 => this._keyControls[InputKeys.KeyboardNumPad9];
        public SingleButtonControl Multiply => this._keyControls[InputKeys.KeyboardMultiply];
        public SingleButtonControl Add => this._keyControls[InputKeys.KeyboardAdd];
        public SingleButtonControl Separator => this._keyControls[InputKeys.KeyboardSeparator];
        public SingleButtonControl Subtract => this._keyControls[InputKeys.KeyboardSubtract];
        public SingleButtonControl Decimal => this._keyControls[InputKeys.KeyboardDecimal];
        public SingleButtonControl Divide => this._keyControls[InputKeys.KeyboardDivide];
        public SingleButtonControl F1 => this._keyControls[InputKeys.KeyboardF1];
        public SingleButtonControl F2 => this._keyControls[InputKeys.KeyboardF2];
        public SingleButtonControl F3 => this._keyControls[InputKeys.KeyboardF3];
        public SingleButtonControl F4 => this._keyControls[InputKeys.KeyboardF4];
        public SingleButtonControl F5 => this._keyControls[InputKeys.KeyboardF5];
        public SingleButtonControl F6 => this._keyControls[InputKeys.KeyboardF6];
        public SingleButtonControl F7 => this._keyControls[InputKeys.KeyboardF7];
        public SingleButtonControl F8 => this._keyControls[InputKeys.KeyboardF8];
        public SingleButtonControl F9 => this._keyControls[InputKeys.KeyboardF9];
        public SingleButtonControl F10 => this._keyControls[InputKeys.KeyboardF10];
        public SingleButtonControl F11 => this._keyControls[InputKeys.KeyboardF11];
        public SingleButtonControl F12 => this._keyControls[InputKeys.KeyboardF12];
        public SingleButtonControl F13 => this._keyControls[InputKeys.KeyboardF13];
        public SingleButtonControl F14 => this._keyControls[InputKeys.KeyboardF14];
        public SingleButtonControl F15 => this._keyControls[InputKeys.KeyboardF15];
        public SingleButtonControl F16 => this._keyControls[InputKeys.KeyboardF16];
        public SingleButtonControl F17 => this._keyControls[InputKeys.KeyboardF17];
        public SingleButtonControl F18 => this._keyControls[InputKeys.KeyboardF18];
        public SingleButtonControl F19 => this._keyControls[InputKeys.KeyboardF19];
        public SingleButtonControl F20 => this._keyControls[InputKeys.KeyboardF20];
        public SingleButtonControl F21 => this._keyControls[InputKeys.KeyboardF21];
        public SingleButtonControl F22 => this._keyControls[InputKeys.KeyboardF22];
        public SingleButtonControl F23 => this._keyControls[InputKeys.KeyboardF23];
        public SingleButtonControl F24 => this._keyControls[InputKeys.KeyboardF24];
        public SingleButtonControl NumLock => this._keyControls[InputKeys.KeyboardNumLock];
        public SingleButtonControl Scroll => this._keyControls[InputKeys.KeyboardScroll];
        public SingleButtonControl LeftShift => this._keyControls[InputKeys.KeyboardLeftShift];
        public SingleButtonControl RightShift => this._keyControls[InputKeys.KeyboardRightShift];
        public SingleButtonControl LeftControl => this._keyControls[InputKeys.KeyboardLeftControl];
        public SingleButtonControl RightControl => this._keyControls[InputKeys.KeyboardRightControl];
        public SingleButtonControl LeftAlt => this._keyControls[InputKeys.KeyboardLeftAlt];
        public SingleButtonControl RightAlt => this._keyControls[InputKeys.KeyboardRightAlt];
        public SingleButtonControl BrowserBack => this._keyControls[InputKeys.KeyboardBrowserBack];
        public SingleButtonControl BrowserForward => this._keyControls[InputKeys.KeyboardBrowserForward];
        public SingleButtonControl BrowserRefresh => this._keyControls[InputKeys.KeyboardBrowserRefresh];
        public SingleButtonControl BrowserStop => this._keyControls[InputKeys.KeyboardBrowserStop];
        public SingleButtonControl BrowserSearch => this._keyControls[InputKeys.KeyboardBrowserSearch];
        public SingleButtonControl BrowserFavorites => this._keyControls[InputKeys.KeyboardBrowserFavorites];
        public SingleButtonControl BrowserHome => this._keyControls[InputKeys.KeyboardBrowserHome];
        public SingleButtonControl VolumeMute => this._keyControls[InputKeys.KeyboardVolumeMute];
        public SingleButtonControl VolumeDown => this._keyControls[InputKeys.KeyboardVolumeDown];
        public SingleButtonControl VolumeUp => this._keyControls[InputKeys.KeyboardVolumeUp];
        public SingleButtonControl MediaNextTrack => this._keyControls[InputKeys.KeyboardMediaNextTrack];
        public SingleButtonControl MediaPreviousTrack => this._keyControls[InputKeys.KeyboardMediaPreviousTrack];
        public SingleButtonControl MediaStop => this._keyControls[InputKeys.KeyboardMediaStop];
        public SingleButtonControl MediaPlayPause => this._keyControls[InputKeys.KeyboardMediaPlayPause];
        public SingleButtonControl LaunchMail => this._keyControls[InputKeys.KeyboardLaunchMail];
        public SingleButtonControl SelectMedia => this._keyControls[InputKeys.KeyboardSelectMedia];
        public SingleButtonControl LaunchApplication1 => this._keyControls[InputKeys.KeyboardLaunchApplication1];
        public SingleButtonControl LaunchApplication2 => this._keyControls[InputKeys.KeyboardLaunchApplication2];
        public SingleButtonControl OemSemicolon => this._keyControls[InputKeys.KeyboardOemSemicolon];
        public SingleButtonControl OemPlus => this._keyControls[InputKeys.KeyboardOemPlus];
        public SingleButtonControl OemComma => this._keyControls[InputKeys.KeyboardOemComma];
        public SingleButtonControl OemMinus => this._keyControls[InputKeys.KeyboardOemMinus];
        public SingleButtonControl OemPeriod => this._keyControls[InputKeys.KeyboardOemPeriod];
        public SingleButtonControl OemQuestion => this._keyControls[InputKeys.KeyboardOemQuestion];
        public SingleButtonControl OemTilde => this._keyControls[InputKeys.KeyboardOemTilde];
        public SingleButtonControl ChatPadGreen => this._keyControls[InputKeys.KeyboardChatPadGreen];
        public SingleButtonControl ChatPadOrange => this._keyControls[InputKeys.KeyboardChatPadOrange];
        public SingleButtonControl OemOpenBrackets => this._keyControls[InputKeys.KeyboardOemOpenBrackets];
        public SingleButtonControl OemPipe => this._keyControls[InputKeys.KeyboardOemPipe];
        public SingleButtonControl OemCloseBrackets => this._keyControls[InputKeys.KeyboardOemCloseBrackets];
        public SingleButtonControl OemQuotes => this._keyControls[InputKeys.KeyboardOemQuotes];
        public SingleButtonControl Oem8 => this._keyControls[InputKeys.KeyboardOem8];
        public SingleButtonControl OemBackslash => this._keyControls[InputKeys.KeyboardOemBackslash];
        public SingleButtonControl ProcessKey => this._keyControls[InputKeys.KeyboardProcessKey];
        public SingleButtonControl OemCopy => this._keyControls[InputKeys.KeyboardOemCopy];
        public SingleButtonControl OemAuto => this._keyControls[InputKeys.KeyboardOemAuto];
        public SingleButtonControl OemEnlW => this._keyControls[InputKeys.KeyboardOemEnlW];
        public SingleButtonControl Attn => this._keyControls[InputKeys.KeyboardAttn];
        public SingleButtonControl Crsel => this._keyControls[InputKeys.KeyboardCrsel];
        public SingleButtonControl Exsel => this._keyControls[InputKeys.KeyboardExsel];
        public SingleButtonControl EraseEof => this._keyControls[InputKeys.KeyboardEraseEof];
        public SingleButtonControl Play => this._keyControls[InputKeys.KeyboardPlay];
        public SingleButtonControl Zoom => this._keyControls[InputKeys.KeyboardZoom];
        public SingleButtonControl Pa1 => this._keyControls[InputKeys.KeyboardPa1];
        public SingleButtonControl OemClear => this._keyControls[InputKeys.KeyboardOemClear];
        #endregion

        #region Derived keys

        public DirectionalPadControl ArrowKeys;

        public DirectionalPadControl WASD;

        #endregion
    }
}
