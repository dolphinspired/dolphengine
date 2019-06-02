using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Input.Controls;
using DolphEngine.Scenery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Demo.Games.InputTester
{
    public class KbScene : IScene
    {
        private readonly Ecosystem Ecosystem;
        private readonly Keycosystem Keycosystem;
        private readonly CameraEntity Camera;
        private readonly DirectiveRenderer Renderer;

        private const int HT = 24; // The height of a key on the rendered keyboard
        private const int SPC = 4; // The spacing between keys on the rendered keyboard

        private const int W_SMALL = HT;
        private const int W_MED = (int)(HT * 1.25);
        private const int W_XMED = (int)(HT * 1.5);
        private const int W_LONG = (int)(HT * 1.75);
        private const int W_XLONG = (int)(HT * 2);
        private const int W_XXLONG = (int)(HT * 2.5);
        private const int W_SPACE = HT * 8;

        private static readonly IReadOnlyDictionary<string, KeyDrawData?> KeySizes = new Dictionary<string, KeyDrawData?>
        {
            #region Data
            { InputKeys.KeyboardNone,           null },
            { InputKeys.KeyboardBack,           new KeyDrawData(1, 13, k => k.Back, W_LONG) },
            { InputKeys.KeyboardTab,            new KeyDrawData(2, 0, k => k.Tab, W_LONG) },
            { InputKeys.KeyboardEnter,          new KeyDrawData(3, 12, k => k.Enter, W_XLONG) },
            { InputKeys.KeyboardPause,          null },
            { InputKeys.KeyboardCapsLock,       new KeyDrawData(3, 0, k => k.CapsLock, W_XLONG) },
            { InputKeys.KeyboardKana,           null },
            { InputKeys.KeyboardKanji,          null },
            { InputKeys.KeyboardEscape,         new KeyDrawData(0, 0, k => k.Escape) },
            { InputKeys.KeyboardImeConvert,     null },
            { InputKeys.KeyboardImeNoConvert,   null },
            { InputKeys.KeyboardSpace,          new KeyDrawData(5, 3, k => k.Space, W_SPACE) },
            { InputKeys.KeyboardPageUp,         null },
            { InputKeys.KeyboardPageDown,       null },
            { InputKeys.KeyboardEnd,            null },
            { InputKeys.KeyboardHome,           null },
            { InputKeys.KeyboardLeft,           null },
            { InputKeys.KeyboardUp,             null },
            { InputKeys.KeyboardRight,          null },
            { InputKeys.KeyboardDown,           null },
            { InputKeys.KeyboardSelect,         null },
            { InputKeys.KeyboardPrint,          null },
            { InputKeys.KeyboardExecute,        null },
            { InputKeys.KeyboardPrintScreen,    null },
            { InputKeys.KeyboardInsert,         null },
            { InputKeys.KeyboardDelete,         null },
            { InputKeys.KeyboardHelp,           null },
            { InputKeys.KeyboardD0,             new KeyDrawData(1, 10, k => k.D0) },
            { InputKeys.KeyboardD1,             new KeyDrawData(1, 1, k => k.D1) },
            { InputKeys.KeyboardD2,             new KeyDrawData(1, 2, k => k.D2) },
            { InputKeys.KeyboardD3,             new KeyDrawData(1, 3, k => k.D3) },
            { InputKeys.KeyboardD4,             new KeyDrawData(1, 4, k => k.D4) },
            { InputKeys.KeyboardD5,             new KeyDrawData(1, 5, k => k.D5) },
            { InputKeys.KeyboardD6,             new KeyDrawData(1, 6, k => k.D6) },
            { InputKeys.KeyboardD7,             new KeyDrawData(1, 7, k => k.D7) },
            { InputKeys.KeyboardD8,             new KeyDrawData(1, 8, k => k.D8) },
            { InputKeys.KeyboardD9,             new KeyDrawData(1, 9, k => k.D9) },
            { InputKeys.KeyboardA,              new KeyDrawData(3, 1, k => k.A) },
            { InputKeys.KeyboardB,              new KeyDrawData(4, 5, k => k.B) },
            { InputKeys.KeyboardC,              new KeyDrawData(4, 3, k => k.C) },
            { InputKeys.KeyboardD,              new KeyDrawData(3, 3, k => k.D) },
            { InputKeys.KeyboardE,              new KeyDrawData(2, 3, k => k.E) },
            { InputKeys.KeyboardF,              new KeyDrawData(3, 4, k => k.F) },
            { InputKeys.KeyboardG,              new KeyDrawData(3, 5, k => k.G) },
            { InputKeys.KeyboardH,              new KeyDrawData(3, 6, k => k.H) },
            { InputKeys.KeyboardI,              new KeyDrawData(2, 8, k => k.I) },
            { InputKeys.KeyboardJ,              new KeyDrawData(3, 7, k => k.J) },
            { InputKeys.KeyboardK,              new KeyDrawData(3, 8, k => k.K) },
            { InputKeys.KeyboardL,              new KeyDrawData(3, 9, k => k.L) },
            { InputKeys.KeyboardM,              new KeyDrawData(4, 7, k => k.M) },
            { InputKeys.KeyboardN,              new KeyDrawData(4, 6, k => k.N) },
            { InputKeys.KeyboardO,              new KeyDrawData(2, 9, k => k.O) },
            { InputKeys.KeyboardP,              new KeyDrawData(2, 10, k => k.P) },
            { InputKeys.KeyboardQ,              new KeyDrawData(2, 1, k => k.Q) },
            { InputKeys.KeyboardR,              new KeyDrawData(2, 4, k => k.R) },
            { InputKeys.KeyboardS,              new KeyDrawData(3, 2, k => k.S) },
            { InputKeys.KeyboardT,              new KeyDrawData(2, 5, k => k.T) },
            { InputKeys.KeyboardU,              new KeyDrawData(2, 7, k => k.U) },
            { InputKeys.KeyboardV,              new KeyDrawData(4, 4, k => k.V) },
            { InputKeys.KeyboardW,              new KeyDrawData(2, 2, k => k.W) },
            { InputKeys.KeyboardX,              new KeyDrawData(4, 2, k => k.X) },
            { InputKeys.KeyboardY,              new KeyDrawData(2, 6, k => k.Y) },
            { InputKeys.KeyboardZ,              new KeyDrawData(4, 1, k => k.Z) },
            { InputKeys.KeyboardLeftWindows,    new KeyDrawData(5, 1, k => k.LeftWindows) },
            { InputKeys.KeyboardRightWindows,   new KeyDrawData(5, 5, k => k.RightWindows) },
            { InputKeys.KeyboardApps,           null },
            { InputKeys.KeyboardSleep,          null },
            { InputKeys.KeyboardNumPad0,        null },
            { InputKeys.KeyboardNumPad1,        null },
            { InputKeys.KeyboardNumPad2,        null },
            { InputKeys.KeyboardNumPad3,        null },
            { InputKeys.KeyboardNumPad4,        null },
            { InputKeys.KeyboardNumPad5,        null },
            { InputKeys.KeyboardNumPad6,        null },
            { InputKeys.KeyboardNumPad7,        null },
            { InputKeys.KeyboardNumPad8,        null },
            { InputKeys.KeyboardNumPad9,        null },
            { InputKeys.KeyboardMultiply,       null },
            { InputKeys.KeyboardAdd,            null },
            { InputKeys.KeyboardSeparator,      null },
            { InputKeys.KeyboardSubtract,       null },
            { InputKeys.KeyboardDecimal,        null },
            { InputKeys.KeyboardDivide,         null },
            { InputKeys.KeyboardF1,             new KeyDrawData(0, 1, k => k.F1) },
            { InputKeys.KeyboardF2,             new KeyDrawData(0, 2, k => k.F2) },
            { InputKeys.KeyboardF3,             new KeyDrawData(0, 3, k => k.F3) },
            { InputKeys.KeyboardF4,             new KeyDrawData(0, 4, k => k.F4) },
            { InputKeys.KeyboardF5,             new KeyDrawData(0, 5, k => k.F5) },
            { InputKeys.KeyboardF6,             new KeyDrawData(0, 6, k => k.F6) },
            { InputKeys.KeyboardF7,             new KeyDrawData(0, 7, k => k.F7) },
            { InputKeys.KeyboardF8,             new KeyDrawData(0, 8, k => k.F8) },
            { InputKeys.KeyboardF9,             new KeyDrawData(0, 9, k => k.F9) },
            { InputKeys.KeyboardF10,            new KeyDrawData(0, 10, k => k.F10) },
            { InputKeys.KeyboardF11,            new KeyDrawData(0, 11, k => k.F11) },
            { InputKeys.KeyboardF12,            new KeyDrawData(0, 12, k => k.F12) },
            { InputKeys.KeyboardF13,            null },
            { InputKeys.KeyboardF14,            null },
            { InputKeys.KeyboardF15,            null },
            { InputKeys.KeyboardF16,            null },
            { InputKeys.KeyboardF17,            null },
            { InputKeys.KeyboardF18,            null },
            { InputKeys.KeyboardF19,            null },
            { InputKeys.KeyboardF20,            null },
            { InputKeys.KeyboardF21,            null },
            { InputKeys.KeyboardF22,            null },
            { InputKeys.KeyboardF23,            null },
            { InputKeys.KeyboardF24,            null },
            { InputKeys.KeyboardNumLock,        null },
            { InputKeys.KeyboardScroll,         null },
            { InputKeys.KeyboardLeftShift,      new KeyDrawData(4, 0, k => k.LeftShift, W_XXLONG) },
            { InputKeys.KeyboardRightShift,     new KeyDrawData(4, 11, k => k.RightShift, W_XXLONG) },
            { InputKeys.KeyboardLeftControl,    new KeyDrawData(5, 0, k => k.LeftControl, W_XMED) },
            { InputKeys.KeyboardRightControl,   new KeyDrawData(5, 7, k => k.RightControl, W_XMED) },
            { InputKeys.KeyboardLeftAlt,        new KeyDrawData(5, 2, k => k.LeftAlt) },
            { InputKeys.KeyboardRightAlt,       new KeyDrawData(5, 4, k => k.RightAlt) },
            { InputKeys.KeyboardBrowserBack,    null },
            { InputKeys.KeyboardBrowserForward, null },
            { InputKeys.KeyboardBrowserRefresh, null },
            { InputKeys.KeyboardBrowserStop,    null },
            { InputKeys.KeyboardBrowserSearch,  null },
            { InputKeys.KeyboardBrowserFavorites, null },
            { InputKeys.KeyboardBrowserHome,    null },
            { InputKeys.KeyboardVolumeMute,     null },
            { InputKeys.KeyboardVolumeDown,     null },
            { InputKeys.KeyboardVolumeUp,       null },
            { InputKeys.KeyboardMediaNextTrack, null },
            { InputKeys.KeyboardMediaPreviousTrack, null },
            { InputKeys.KeyboardMediaStop,      null },
            { InputKeys.KeyboardMediaPlayPause, null },
            { InputKeys.KeyboardLaunchMail,     null },
            { InputKeys.KeyboardSelectMedia,    null },
            { InputKeys.KeyboardLaunchApplication1, null },
            { InputKeys.KeyboardLaunchApplication2, null },
            { InputKeys.KeyboardOemSemicolon,   new KeyDrawData(3, 10, k => k.OemSemicolon) },
            { InputKeys.KeyboardOemPlus,        new KeyDrawData(1, 12, k => k.OemPlus) },
            { InputKeys.KeyboardOemComma,       new KeyDrawData(4, 8, k => k.OemComma) },
            { InputKeys.KeyboardOemMinus,       new KeyDrawData(1, 11, k => k.OemMinus) },
            { InputKeys.KeyboardOemPeriod,      new KeyDrawData(4, 9, k => k.OemPeriod) },
            { InputKeys.KeyboardOemQuestion,    new KeyDrawData(4, 10, k => k.OemQuestion) },
            { InputKeys.KeyboardOemTilde,       new KeyDrawData(1, 0, k => k.OemTilde) },
            { InputKeys.KeyboardChatPadGreen,   null },
            { InputKeys.KeyboardChatPadOrange,  null },
            { InputKeys.KeyboardOemOpenBrackets, new KeyDrawData(2, 11, k => k.OemOpenBrackets) },
            { InputKeys.KeyboardOemPipe,        new KeyDrawData(2, 13, k => k.OemPipe, W_MED) },
            { InputKeys.KeyboardOemCloseBrackets, new KeyDrawData(2, 12, k => k.OemCloseBrackets) },
            { InputKeys.KeyboardOemQuotes,      new KeyDrawData(3, 11, k => k.OemQuotes) },
            { InputKeys.KeyboardOem8,           null },
            { InputKeys.KeyboardOemBackslash,   null },
            { InputKeys.KeyboardProcessKey,     null },
            { InputKeys.KeyboardOemCopy,        null },
            { InputKeys.KeyboardOemAuto,        null },
            { InputKeys.KeyboardOemEnlW,        null },
            { InputKeys.KeyboardAttn,           null },
            { InputKeys.KeyboardCrsel,          null },
            { InputKeys.KeyboardExsel,          null },
            { InputKeys.KeyboardEraseEof,       null },
            { InputKeys.KeyboardPlay,           null },
            { InputKeys.KeyboardZoom,           null },
            { InputKeys.KeyboardPa1,            null },
            { InputKeys.KeyboardOemClear,       null },
            #endregion
        };

        private readonly Dictionary<int, List<KeyDrawData>> DrawnKeysByRow = new Dictionary<int, List<KeyDrawData>>();

        public KbScene(
            Ecosystem ecosystem,
            Keycosystem keycosystem,
            CameraEntity camera,
            DirectiveRenderer renderer)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.Camera = camera;
            this.Renderer = renderer;
        }

        public void Load()
        {
            this.LoadEntities();
            this.LoadControls();
        }

        public void Unload()
        {
            this.Keycosystem.RemoveControlScheme("KeyHighlighter");
        }

        public void Update()
        {
            this.Ecosystem.Update();
            this.Keycosystem.Update();
        }

        public void Draw()
        {
            this.Ecosystem.Draw();
        }

        private void LoadEntities()
        {
            var drawnKeys = KeySizes
                .Where(kvp => kvp.Value != null)
                .Select(kvp =>
                {
                    var kdd = kvp.Value.Value;
                    kdd.KeyToRead = kvp.Key;
                    return kdd;
                })
                .OrderBy(kdd => kdd.Row)
                .ThenBy(kdd => kdd.Col);

            foreach (var drawnKey in drawnKeys)
            {
                if (!this.DrawnKeysByRow.TryGetValue(drawnKey.Row, out var keys))
                {
                    keys = new List<KeyDrawData>();
                    this.DrawnKeysByRow[drawnKey.Row] = keys;
                }

                keys.Add(drawnKey);
            }

            var keyboard = this.Keycosystem.GetController<StandardKeyboard>(1);
            var controlScheme = new ControlScheme();

            foreach (var kvp in this.DrawnKeysByRow)
            {
                var row = kvp.Key;
                var x = 0;

                foreach (var kdd in kvp.Value)
                {
                    var keyEntity = new Entity();
                    var rect = new Rect2d(x, row * (HT + SPC), kdd.Width, HT);
                    keyEntity.Space = rect;
                    keyEntity.AddComponent(new PolygonComponent {
                        Polygon = new Rect2d(0, 0, rect.Size.Width, rect.Size.Height).ToPolygon(),
                        Color = 0x00FFFFFF });
                    keyEntity.AddComponent<DrawComponent>();
                    this.Ecosystem.AddEntity(keyEntity);

                    var keyboardKey = kdd.KeyboardKey(keyboard);
                    controlScheme.AddControl(() => keyboardKey.JustPressed, () => keyEntity.GetComponent<PolygonComponent>().Color = 0x000000FF);
                    controlScheme.AddControl(() => keyboardKey.JustReleased, () => keyEntity.GetComponent<PolygonComponent>().Color = 0x00FFFFFF);

                    x += kdd.Width + SPC;
                }
            }

            this.Keycosystem.AddControlScheme("KeyHighlighter", controlScheme);

            this.Ecosystem
                .AddEntity(this.Camera)
                .AddHandler<PolygonHandler>()
                .AddHandler(new DrawHandler(this.Renderer));

            // todo: none of this works to move the camera?
            // this.Camera.Lens.Pan = new Vector2d(200, 100);
            this.Camera.Space.Position.X += 200;
            this.Camera.Space.Position.Y += 100;
        }

        private void LoadControls()
        {
            
        }

        private struct KeyDrawData
        {
            public KeyDrawData(int row, int col, Func<StandardKeyboard, SingleButtonControl> keyboardKey, int width = W_SMALL)
            {
                this.Row = row;
                this.Col = col;
                this.Width = width;
                this.KeyToRead = null;
                this.KeyboardKey = keyboardKey;
            }

            public readonly int Row;

            public readonly int Col;

            public int Width;

            public string KeyToRead;

            public readonly Func<StandardKeyboard, SingleButtonControl> KeyboardKey;
        }
    }
}
