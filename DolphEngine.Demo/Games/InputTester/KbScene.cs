using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Scenery;
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

        private const int W_KEY = HT;
        private const int W_WIDE = (int)(HT * 1.25);
        private const int W_VWIDE = (int)(HT * 1.25);
        private const int W_SPACE = HT * 8;

        private static readonly IReadOnlyDictionary<string, KeyDrawData?> KeySizes = new Dictionary<string, KeyDrawData?>
        {
            #region Data
            { InputKeys.KeyboardNone,           null },
            { InputKeys.KeyboardBack,           new KeyDrawData(1, 13, W_WIDE) },
            { InputKeys.KeyboardTab,            new KeyDrawData(2, 0, W_WIDE) },
            { InputKeys.KeyboardEnter,          new KeyDrawData(3, 12, W_WIDE) },
            { InputKeys.KeyboardPause,          null },
            { InputKeys.KeyboardCapsLock,       new KeyDrawData(3, 0, W_WIDE) },
            { InputKeys.KeyboardKana,           null },
            { InputKeys.KeyboardKanji,          null },
            { InputKeys.KeyboardEscape,         new KeyDrawData(0, 0, W_KEY) },
            { InputKeys.KeyboardImeConvert,     null },
            { InputKeys.KeyboardImeNoConvert,   null },
            { InputKeys.KeyboardSpace,          new KeyDrawData(5, 3, W_SPACE) },
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
            { InputKeys.KeyboardD0,             new KeyDrawData(1, 10, W_KEY) },
            { InputKeys.KeyboardD1,             new KeyDrawData(1, 1, W_KEY) },
            { InputKeys.KeyboardD2,             new KeyDrawData(1, 2, W_KEY) },
            { InputKeys.KeyboardD3,             new KeyDrawData(1, 3, W_KEY) },
            { InputKeys.KeyboardD4,             new KeyDrawData(1, 4, W_KEY) },
            { InputKeys.KeyboardD5,             new KeyDrawData(1, 5, W_KEY) },
            { InputKeys.KeyboardD6,             new KeyDrawData(1, 6, W_KEY) },
            { InputKeys.KeyboardD7,             new KeyDrawData(1, 7, W_KEY) },
            { InputKeys.KeyboardD8,             new KeyDrawData(1, 8, W_KEY) },
            { InputKeys.KeyboardD9,             new KeyDrawData(1, 9, W_KEY) },
            { InputKeys.KeyboardA,              new KeyDrawData(3, 1, W_KEY) },
            { InputKeys.KeyboardB,              new KeyDrawData(4, 5, W_KEY) },
            { InputKeys.KeyboardC,              new KeyDrawData(4, 3, W_KEY) },
            { InputKeys.KeyboardD,              new KeyDrawData(3, 3, W_KEY) },
            { InputKeys.KeyboardE,              new KeyDrawData(2, 3, W_KEY) },
            { InputKeys.KeyboardF,              new KeyDrawData(3, 4, W_KEY) },
            { InputKeys.KeyboardG,              new KeyDrawData(3, 5, W_KEY) },
            { InputKeys.KeyboardH,              new KeyDrawData(3, 6, W_KEY) },
            { InputKeys.KeyboardI,              new KeyDrawData(2, 8, W_KEY) },
            { InputKeys.KeyboardJ,              new KeyDrawData(3, 7, W_KEY) },
            { InputKeys.KeyboardK,              new KeyDrawData(3, 8, W_KEY) },
            { InputKeys.KeyboardL,              new KeyDrawData(3, 9, W_KEY) },
            { InputKeys.KeyboardM,              new KeyDrawData(4, 7, W_KEY) },
            { InputKeys.KeyboardN,              new KeyDrawData(4, 6, W_KEY) },
            { InputKeys.KeyboardO,              new KeyDrawData(2, 9, W_KEY) },
            { InputKeys.KeyboardP,              new KeyDrawData(2, 10, W_KEY) },
            { InputKeys.KeyboardQ,              new KeyDrawData(2, 1, W_KEY) },
            { InputKeys.KeyboardR,              new KeyDrawData(2, 4, W_KEY) },
            { InputKeys.KeyboardS,              new KeyDrawData(3, 2, W_KEY) },
            { InputKeys.KeyboardT,              new KeyDrawData(2, 5, W_KEY) },
            { InputKeys.KeyboardU,              new KeyDrawData(2, 7, W_KEY) },
            { InputKeys.KeyboardV,              new KeyDrawData(4, 4, W_KEY) },
            { InputKeys.KeyboardW,              new KeyDrawData(2, 2, W_KEY) },
            { InputKeys.KeyboardX,              new KeyDrawData(4, 2, W_KEY) },
            { InputKeys.KeyboardY,              new KeyDrawData(2, 6, W_KEY) },
            { InputKeys.KeyboardZ,              new KeyDrawData(4, 1, W_KEY) },
            { InputKeys.KeyboardLeftWindows,    new KeyDrawData(5, 1, W_KEY) },
            { InputKeys.KeyboardRightWindows,   new KeyDrawData(5, 5, W_KEY) },
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
            { InputKeys.KeyboardF1,             new KeyDrawData(0, 1, W_KEY) },
            { InputKeys.KeyboardF2,             new KeyDrawData(0, 2, W_KEY) },
            { InputKeys.KeyboardF3,             new KeyDrawData(0, 3, W_KEY) },
            { InputKeys.KeyboardF4,             new KeyDrawData(0, 4, W_KEY) },
            { InputKeys.KeyboardF5,             new KeyDrawData(0, 5, W_KEY) },
            { InputKeys.KeyboardF6,             new KeyDrawData(0, 6, W_KEY) },
            { InputKeys.KeyboardF7,             new KeyDrawData(0, 7, W_KEY) },
            { InputKeys.KeyboardF8,             new KeyDrawData(0, 8, W_KEY) },
            { InputKeys.KeyboardF9,             new KeyDrawData(0, 9, W_KEY) },
            { InputKeys.KeyboardF10,            new KeyDrawData(0, 10, W_KEY) },
            { InputKeys.KeyboardF11,            new KeyDrawData(0, 11, W_KEY) },
            { InputKeys.KeyboardF12,            new KeyDrawData(0, 12, W_KEY) },
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
            { InputKeys.KeyboardLeftShift,      new KeyDrawData(4, 0, W_VWIDE) },
            { InputKeys.KeyboardRightShift,     new KeyDrawData(4, 11, W_VWIDE) },
            { InputKeys.KeyboardLeftControl,    new KeyDrawData(5, 0, W_WIDE) },
            { InputKeys.KeyboardRightControl,   new KeyDrawData(5, 7, W_WIDE) },
            { InputKeys.KeyboardLeftAlt,        new KeyDrawData(5, 2, W_KEY) },
            { InputKeys.KeyboardRightAlt,       new KeyDrawData(5, 4, W_KEY) },
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
            { InputKeys.KeyboardOemSemicolon,   new KeyDrawData(3, 10, W_KEY) },
            { InputKeys.KeyboardOemPlus,        new KeyDrawData(1, 12, W_KEY) },
            { InputKeys.KeyboardOemComma,       new KeyDrawData(4, 8, W_KEY) },
            { InputKeys.KeyboardOemMinus,       new KeyDrawData(1, 11, W_KEY) },
            { InputKeys.KeyboardOemPeriod,      new KeyDrawData(4, 9, W_KEY) },
            { InputKeys.KeyboardOemQuestion,    new KeyDrawData(4, 10, W_KEY) },
            { InputKeys.KeyboardOemTilde,       new KeyDrawData(1, 0, W_KEY) },
            { InputKeys.KeyboardChatPadGreen,   null },
            { InputKeys.KeyboardChatPadOrange,  null },
            { InputKeys.KeyboardOemOpenBrackets, new KeyDrawData(1, 11, W_KEY) },
            { InputKeys.KeyboardOemPipe,        new KeyDrawData(1, 13, W_WIDE) },
            { InputKeys.KeyboardOemCloseBrackets, new KeyDrawData(1, 12, W_KEY) },
            { InputKeys.KeyboardOemQuotes,      new KeyDrawData(3, 11, W_KEY) },
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

                    x += kdd.Width + SPC;
                }
            }

            this.Ecosystem
                .AddEntity(this.Camera)
                .AddHandler<PolygonHandler>()
                .AddHandler(new DrawHandler(this.Renderer));

            // todo: none of this works to move the camera?
            this.Camera.Lens.Pan = new Vector2d(200, 100);
            this.Camera.Space.Position.X = 200;
            this.Camera.Space.Position.Y = 100;
        }

        private void LoadControls()
        {
            
        }

        private struct KeyDrawData
        {
            public KeyDrawData(int row, int col, int width)
            {
                this.Row = row;
                this.Col = col;
                this.Width = width;
                this.KeyToRead = null;
            }

            public readonly int Row;

            public readonly int Col;

            public readonly int Width;

            public string KeyToRead;
        }
    }
}
