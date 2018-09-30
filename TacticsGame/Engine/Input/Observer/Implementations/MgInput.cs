using Microsoft.Xna.Framework.Input;
using System;

namespace TacticsGame.Engine.Input.Observer.Implementations
{
    namespace MgInput
    {
        public static class MgInputExtensions
        {
            // [   0... 999] - Keyboard keys and combinations
            // [1000...1999] - Mouse inputs and combinations
            // [2000...2999] - Player 1 Gamepad inputs and combinations
            // [3000...3999] - Player 2 Gamepad inputs and combinations
            // ...etc., up to as many gamepads as MonoGame supports
            internal const int Buffer = 1000;

            /// <summary>
            /// Translates a MonoGame <see cref="Keys"/> value into a unique integer to use with <see cref="MonoGameObserver"/>.
            /// </summary>
            public static int ToGenericInput(this Keys key)
            {
                return (int)key;
            }

            /// <summary>
            /// Translates a MonoGame <see cref="Buttons"/> value, along with a 1-indexed player number, into a unique integer 
            /// to use with <see cref="MonoGameObserver"/>
            /// </summary>
            public static int ToGenericInput(this Buttons button, int player)
            {
                return (int)((Buffer * (player + 1)) + Math.Log((double)button, 2));
            }

            internal static int GetPlayer(int genericInput)
            {
                return (genericInput / Buffer) - 1;
            }

            internal static int GetPlayerEquivalent(int genericInput, int player)
            {
                return (Buffer * (player + 1)) + (genericInput % Buffer);
            }
        }

        public static class Keyboard
        {
            public const int None = 0;
            public const int Back = 8;
            public const int Tab = 9;
            public const int Enter = 13;
            public const int Pause = 19;
            public const int CapsLock = 20;
            public const int Kana = 21;
            public const int Kanji = 25;
            public const int Escape = 27;
            public const int ImeConvert = 28;
            public const int ImeNoConvert = 29;
            public const int Space = 32;
            public const int PageUp = 33;
            public const int PageDown = 34;
            public const int End = 35;
            public const int Home = 36;
            public const int Left = 37;
            public const int Up = 38;
            public const int Right = 39;
            public const int Down = 40;
            public const int Select = 41;
            public const int Print = 42;
            public const int Execute = 43;
            public const int PrintScreen = 44;
            public const int Insert = 45;
            public const int Delete = 46;
            public const int Help = 47;
            public const int D0 = 48;
            public const int D1 = 49;
            public const int D2 = 50;
            public const int D3 = 51;
            public const int D4 = 52;
            public const int D5 = 53;
            public const int D6 = 54;
            public const int D7 = 55;
            public const int D8 = 56;
            public const int D9 = 57;
            public const int A = 65;
            public const int B = 66;
            public const int C = 67;
            public const int D = 68;
            public const int E = 69;
            public const int F = 70;
            public const int G = 71;
            public const int H = 72;
            public const int I = 73;
            public const int J = 74;
            public const int K = 75;
            public const int L = 76;
            public const int M = 77;
            public const int N = 78;
            public const int O = 79;
            public const int P = 80;
            public const int Q = 81;
            public const int R = 82;
            public const int S = 83;
            public const int T = 84;
            public const int U = 85;
            public const int V = 86;
            public const int W = 87;
            public const int X = 88;
            public const int Y = 89;
            public const int Z = 90;
            public const int LeftWindows = 91;
            public const int RightWindows = 92;
            public const int Apps = 93;
            public const int Sleep = 95;
            public const int NumPad0 = 96;
            public const int NumPad1 = 97;
            public const int NumPad2 = 98;
            public const int NumPad3 = 99;
            public const int NumPad4 = 100;
            public const int NumPad5 = 101;
            public const int NumPad6 = 102;
            public const int NumPad7 = 103;
            public const int NumPad8 = 104;
            public const int NumPad9 = 105;
            public const int Multiply = 106;
            public const int Add = 107;
            public const int Separator = 108;
            public const int Subtract = 109;
            public const int Decimal = 110;
            public const int Divide = 111;
            public const int F1 = 112;
            public const int F2 = 113;
            public const int F3 = 114;
            public const int F4 = 115;
            public const int F5 = 116;
            public const int F6 = 117;
            public const int F7 = 118;
            public const int F8 = 119;
            public const int F9 = 120;
            public const int F10 = 121;
            public const int F11 = 122;
            public const int F12 = 123;
            public const int F13 = 124;
            public const int F14 = 125;
            public const int F15 = 126;
            public const int F16 = 127;
            public const int F17 = 128;
            public const int F18 = 129;
            public const int F19 = 130;
            public const int F20 = 131;
            public const int F21 = 132;
            public const int F22 = 133;
            public const int F23 = 134;
            public const int F24 = 135;
            public const int NumLock = 144;
            public const int Scroll = 145;
            public const int LeftShift = 160;
            public const int RightShift = 161;
            public const int LeftControl = 162;
            public const int RightControl = 163;
            public const int LeftAlt = 164;
            public const int RightAlt = 165;
            public const int BrowserBack = 166;
            public const int BrowserForward = 167;
            public const int BrowserRefresh = 168;
            public const int BrowserStop = 169;
            public const int BrowserSearch = 170;
            public const int BrowserFavorites = 171;
            public const int BrowserHome = 172;
            public const int VolumeMute = 173;
            public const int VolumeDown = 174;
            public const int VolumeUp = 175;
            public const int MediaNextTrack = 176;
            public const int MediaPreviousTrack = 177;
            public const int MediaStop = 178;
            public const int MediaPlayPause = 179;
            public const int LaunchMail = 180;
            public const int SelectMedia = 181;
            public const int LaunchApplication1 = 182;
            public const int LaunchApplication2 = 183;
            public const int OemSemicolon = 186;
            public const int OemPlus = 187;
            public const int OemComma = 188;
            public const int OemMinus = 189;
            public const int OemPeriod = 190;
            public const int OemQuestion = 191;
            public const int OemTilde = 192;
            public const int ChatPadGreen = 202;
            public const int ChatPadOrange = 203;
            public const int OemOpenBrackets = 219;
            public const int OemPipe = 220;
            public const int OemCloseBrackets = 221;
            public const int OemQuotes = 222;
            public const int Oem8 = 223;
            public const int OemBackslash = 226;
            public const int ProcessKey = 229;
            public const int OemCopy = 242;
            public const int OemAuto = 243;
            public const int OemEnlW = 244;
            public const int Attn = 246;
            public const int Crsel = 247;
            public const int Exsel = 248;
            public const int EraseEof = 249;
            public const int Play = 250;
            public const int Zoom = 251;
            public const int Pa1 = 253;
            public const int OemClear = 254;

            // Combination Inputs

            /// <summary>
            /// Watch the WASD keys as a single <see cref="KeyState"/>, like a directional pad.
            /// </summary>
            public const int WASD = 300;

            /// <summary>
            /// Watch the arrow keys as a single <see cref="KeyState"/>, like a directional pad.
            /// </summary>
            public const int Arrows = 301;
        }

        public static class Mouse
        {
            private const int buffer = MgInputExtensions.Buffer;

            public const int Button1 = buffer + 1;
            public const int Button2 = buffer + 2;
            public const int Button3 = buffer + 3;
            public const int Button4 = buffer + 4;
            public const int Button5 = buffer + 5;
            public const int CursorX = buffer + 6;
            public const int CursorY = buffer + 7;
            public const int ScrollX = buffer + 8;
            public const int ScrollY = buffer + 9;

            // Combination Inputs

            /// <summary>
            /// Watch the mouse cursor's position as a single <see cref="KeyState"/>,
            /// where <see cref="KeyState.DigitalX"/> and <see cref="KeyState.DigitalY"/> represent
            /// the cursor's position within the game window.
            /// </summary>
            public const int Cursor = buffer + 10;

            /// <summary>
            /// Watch the scroll wheel's vertical and horizontal values as a single <see cref="KeyState"/>,
            /// where <see cref="KeyState.DigitalX"/> represents horizontal scroll and <see cref="KeyState.DigitalY"/> 
            /// represents vertical scroll.
            /// </summary>
            public const int Scroll = buffer + 11;
        }

        namespace GamePad
        {
            public static class Player1
            {
                private const int buffer = MgInputExtensions.Buffer * 2;

                public const int DPadUp = buffer + 0;
                public const int DPadDown = buffer + 1;
                public const int DPadLeft = buffer + 2;
                public const int DPadRight = buffer + 3;
                public const int Start = buffer + 4;
                public const int Back = buffer + 5;
                public const int LeftStick = buffer + 6;
                public const int RightStick = buffer + 7;
                public const int LeftShoulder = buffer + 8;
                public const int RightShoulder = buffer + 9;
                public const int BigButton = buffer + 11;
                public const int A = buffer + 12;
                public const int B = buffer + 13;
                public const int X = buffer + 14;
                public const int Y = buffer + 15;
                public const int RightTrigger = buffer + 22;
                public const int LeftTrigger = buffer + 23;
                public const int RightThumbstickUp = buffer + 24;
                public const int RightThumbstickDown = buffer + 25;
                public const int RightThumbstickRight = buffer + 26;
                public const int RightThumbstickLeft = buffer + 27;
                public const int LeftThumbstickUp = buffer + 28;
                public const int LeftThumbstickDown = buffer + 29;
                public const int LeftThumbstickRight = buffer + 30;
                public const int LeftThumbstickLeft = buffer + 21;

                // Combination Inputs

                /// <summary>
                /// Watch the four DPad buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int DPad = buffer + 100;

                /// <summary>
                /// Watch the left thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int LeftThumbstick = buffer + 101;

                /// <summary>
                /// Watch the right thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int RightThumbstick = buffer + 102;

                /// <summary>
                /// Watch the A, B, X, and Y buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int ABXY = buffer + 103;

                /// <summary>
                /// Watch both triggers as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/> and
                /// <see cref="KeyState.AnalogX"/> represent the left trigger, and <see cref="KeyState.DigitalY"/> and
                /// <see cref="KeyState.AnalogY"/> represent the right trigger.
                /// </summary>
                public const int Triggers = buffer + 104;
            }

            public static class Player2
            {
                private const int buffer = MgInputExtensions.Buffer * 3;

                public const int DPadUp = buffer + 0;
                public const int DPadDown = buffer + 1;
                public const int DPadLeft = buffer + 2;
                public const int DPadRight = buffer + 3;
                public const int Start = buffer + 4;
                public const int Back = buffer + 5;
                public const int LeftStick = buffer + 6;
                public const int RightStick = buffer + 7;
                public const int LeftShoulder = buffer + 8;
                public const int RightShoulder = buffer + 9;
                public const int BigButton = buffer + 11;
                public const int A = buffer + 12;
                public const int B = buffer + 13;
                public const int X = buffer + 14;
                public const int Y = buffer + 15;
                public const int RightTrigger = buffer + 22;
                public const int LeftTrigger = buffer + 23;
                public const int RightThumbstickUp = buffer + 24;
                public const int RightThumbstickDown = buffer + 25;
                public const int RightThumbstickRight = buffer + 26;
                public const int RightThumbstickLeft = buffer + 27;
                public const int LeftThumbstickUp = buffer + 28;
                public const int LeftThumbstickDown = buffer + 29;
                public const int LeftThumbstickRight = buffer + 30;
                public const int LeftThumbstickLeft = buffer + 21;

                // Combination Inputs

                /// <summary>
                /// Watch the four DPad buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int DPad = buffer + 100;

                /// <summary>
                /// Watch the left thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int LeftThumbstick = buffer + 101;

                /// <summary>
                /// Watch the right thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int RightThumbstick = buffer + 102;

                /// <summary>
                /// Watch the A, B, X, and Y buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int ABXY = buffer + 103;

                /// <summary>
                /// Watch both triggers as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/> and
                /// <see cref="KeyState.AnalogX"/> represent the left trigger, and <see cref="KeyState.DigitalY"/> and
                /// <see cref="KeyState.AnalogY"/> represent the right trigger.
                /// </summary>
                public const int Triggers = buffer + 104;
            }

            public static class Player3
            {
                private const int buffer = MgInputExtensions.Buffer * 4;

                public const int DPadUp = buffer + 0;
                public const int DPadDown = buffer + 1;
                public const int DPadLeft = buffer + 2;
                public const int DPadRight = buffer + 3;
                public const int Start = buffer + 4;
                public const int Back = buffer + 5;
                public const int LeftStick = buffer + 6;
                public const int RightStick = buffer + 7;
                public const int LeftShoulder = buffer + 8;
                public const int RightShoulder = buffer + 9;
                public const int BigButton = buffer + 11;
                public const int A = buffer + 12;
                public const int B = buffer + 13;
                public const int X = buffer + 14;
                public const int Y = buffer + 15;
                public const int RightTrigger = buffer + 22;
                public const int LeftTrigger = buffer + 23;
                public const int RightThumbstickUp = buffer + 24;
                public const int RightThumbstickDown = buffer + 25;
                public const int RightThumbstickRight = buffer + 26;
                public const int RightThumbstickLeft = buffer + 27;
                public const int LeftThumbstickUp = buffer + 28;
                public const int LeftThumbstickDown = buffer + 29;
                public const int LeftThumbstickRight = buffer + 30;
                public const int LeftThumbstickLeft = buffer + 21;

                // Combination Inputs

                /// <summary>
                /// Watch the four DPad buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int DPad = buffer + 100;

                /// <summary>
                /// Watch the left thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int LeftThumbstick = buffer + 101;

                /// <summary>
                /// Watch the right thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int RightThumbstick = buffer + 102;

                /// <summary>
                /// Watch the A, B, X, and Y buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int ABXY = buffer + 103;

                /// <summary>
                /// Watch both triggers as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/> and
                /// <see cref="KeyState.AnalogX"/> represent the left trigger, and <see cref="KeyState.DigitalY"/> and
                /// <see cref="KeyState.AnalogY"/> represent the right trigger.
                /// </summary>
                public const int Triggers = buffer + 104;
            }

            public static class Player4
            {
                private const int buffer = MgInputExtensions.Buffer * 5;

                public const int DPadUp = buffer + 0;
                public const int DPadDown = buffer + 1;
                public const int DPadLeft = buffer + 2;
                public const int DPadRight = buffer + 3;
                public const int Start = buffer + 4;
                public const int Back = buffer + 5;
                public const int LeftStick = buffer + 6;
                public const int RightStick = buffer + 7;
                public const int LeftShoulder = buffer + 8;
                public const int RightShoulder = buffer + 9;
                public const int BigButton = buffer + 11;
                public const int A = buffer + 12;
                public const int B = buffer + 13;
                public const int X = buffer + 14;
                public const int Y = buffer + 15;
                public const int RightTrigger = buffer + 22;
                public const int LeftTrigger = buffer + 23;
                public const int RightThumbstickUp = buffer + 24;
                public const int RightThumbstickDown = buffer + 25;
                public const int RightThumbstickRight = buffer + 26;
                public const int RightThumbstickLeft = buffer + 27;
                public const int LeftThumbstickUp = buffer + 28;
                public const int LeftThumbstickDown = buffer + 29;
                public const int LeftThumbstickRight = buffer + 30;
                public const int LeftThumbstickLeft = buffer + 21;

                // Combination Inputs

                /// <summary>
                /// Watch the four DPad buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int DPad = buffer + 100;

                /// <summary>
                /// Watch the left thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int LeftThumbstick = buffer + 101;

                /// <summary>
                /// Watch the right thumbstick's position as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to determine direction, <see cref="KeyState.AnalogX"/>
                /// and <see cref="KeyState.AnalogY"/> can be used to determine both magnitude and direction.
                /// </summary>
                public const int RightThumbstick = buffer + 102;

                /// <summary>
                /// Watch the A, B, X, and Y buttons as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/>
                /// and <see cref="KeyState.DigitalY"/> can be used to discern direction.
                /// </summary>
                public const int ABXY = buffer + 103;

                /// <summary>
                /// Watch both triggers as a single <see cref="KeyState"/>, where <see cref="KeyState.DigitalX"/> and
                /// <see cref="KeyState.AnalogX"/> represent the left trigger, and <see cref="KeyState.DigitalY"/> and
                /// <see cref="KeyState.AnalogY"/> represent the right trigger.
                /// </summary>
                public const int Triggers = buffer + 104;
            }
        }
    }
}
