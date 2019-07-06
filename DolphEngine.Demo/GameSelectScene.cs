using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Handlers;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Scenery;
using DolphEngine.UI.Containers;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Demo
{
    public class GameSelectScene : IScene
    {
        protected readonly Ecosystem Ecosystem;
        protected readonly Keycosystem Keycosystem;
        protected readonly Director Director;
        protected readonly DebugLogger DebugLogger;
        protected readonly DirectiveRenderer Renderer;
        protected readonly FpsCounter FpsCounter;
        protected readonly Window Window;

        protected readonly Viewport2d Camera;

        private readonly string[] _selectableScenes = new[]
        {
            Scenes.TestMapScene,
            Scenes.DogTreasureHunt,
            Scenes.InputTester,
        };

        private int _selectedIndex;
        private readonly List<Entity> _selectableEntities = new List<Entity>();

        public GameSelectScene(
            Ecosystem ecosystem,
            Keycosystem keycosystem,
            Director director,
            DebugLogger debugLogger,
            DirectiveRenderer renderer,
            FpsCounter fpsCounter,
            Window window)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.Director = director;
            this.DebugLogger = debugLogger;
            this.Renderer = renderer;
            this.FpsCounter = fpsCounter;
            this.Window = window;

            this.Camera = renderer.GetViewport("default");
        }

        public void Load()
        {
            var viewTopLeft = this.Camera.GetAnchorPosition(Anchor2d.TopLeft);
            viewTopLeft.Shift(10, 10); // Padding from edge of screen

            var title = new TextBox
            {
                Text = "Select a scene:",
                Color = new ColorRGBA(255, 255, 255),
                Font = "Assets/Zelda12",
                Rect = new Rect2d(viewTopLeft, Size2d.Zero)
            };
            this.Window.Children.Add(title);

            var cursor = new Entity(new Rect2d(0, 0, 7, 11, Anchor2d.MiddleRight));
            cursor.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Glyphs, Index = 1 });
            this.Ecosystem.AddEntity("Cursor", cursor);

            int i = 0;
            foreach (var sceneName in this._selectableScenes)
            {
                var option = new Entity(new Rect2d(title.GetAnchorPosition(Anchor2d.TopLeft) + new Vector2d(20, ++i * 30), Size2d.Zero, Anchor2d.MiddleLeft));
                option.AddComponent(new TextComponent { Text = sceneName, FontAssetName = "Assets/Zelda12", Color = new ColorRGBA(255, 255, 255) });

                var selectable = new SelectableItemComponent();
                selectable.OnFocus = () => cursor.MoveTo(option.Rect.GetOriginPosition() + new Vector2d(-5, 6)); // text alignment is broken af right now
                selectable.OnBlur = () => { /* Nothing right now! */ };
                option.AddComponent(selectable);

                if (i == 1)
                {
                    // Make the first item selected by default
                    selectable.Selected = true;
                    selectable.OnFocus();
                    _selectedIndex = 0;
                }

                this.Ecosystem.AddEntity($"scene:{sceneName}", option);
                this._selectableEntities.Add(option);
            }

            this.Ecosystem
                .AddHandler<TextHandler>()
                .AddHandler<SpriteHandler>();

            var k = this.Keycosystem.GetController<StandardKeyboard>(1);

            var controls = new ControlScheme()
                .AddControl(() => k.Down.JustPressed, () =>
                {
                    if (this._selectedIndex < this._selectableEntities.Count - 1)
                    {
                        var sel = this._selectableEntities[this._selectedIndex].GetComponent<SelectableItemComponent>();
                        sel.Selected = false;
                        sel.OnBlur();

                        this._selectedIndex++;

                        sel = this._selectableEntities[this._selectedIndex].GetComponent<SelectableItemComponent>();
                        sel.Selected = true;
                        sel.OnFocus();
                    }
                })
                .AddControl(() => k.Up.JustPressed, () =>
                {
                    if (this._selectedIndex > 0)
                    {
                        var sel = this._selectableEntities[this._selectedIndex].GetComponent<SelectableItemComponent>();
                        sel.Selected = false;
                        sel.OnBlur();

                        this._selectedIndex--;

                        sel = this._selectableEntities[this._selectedIndex].GetComponent<SelectableItemComponent>();
                        sel.Selected = true;
                        sel.OnFocus();
                    }
                })
                .AddControl(() => k.Enter.JustPressed, () =>
                {
                    var selectedSceneName = this._selectableScenes[this._selectedIndex];
                    this.Director.LoadScene(selectedSceneName);
                })
                .AddControl(() => k.WASD.IsPressed, () => ControlSchemes.PanCamera(this.Camera, k.WASD));

            this.Keycosystem.AddControlScheme("SceneSelect", controls);
            this.Keycosystem.AddControlScheme("DebugNav", ControlSchemes.DebugNavigation(this.DebugLogger, k));
            this.DebugLogger.AddPage(
                () => $"Ecosystem Directives: {this.Ecosystem.Directives.Count()}",
                () => $"Window Directives: {this.Window.Directives.Count()}");
            

            this.Renderer
                .AddViewChannel("default", this.Ecosystem)
                .AddViewChannel("default", this.Window);
        }

        public void Unload()
        {
            
        }

        public void Update()
        {
            this.Ecosystem.Update();
            this.Keycosystem.Update();
            this.FpsCounter.Update();
        }

        public void Draw()
        {
            this.Renderer.Draw();
            this.DebugLogger.Draw();
            this.FpsCounter.Draw();
        }
    }
}
