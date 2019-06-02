using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Graphics;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Scenery;
using System.Collections.Generic;

namespace DolphEngine.Demo
{
    public class GameSelectScene : IScene
    {
        protected readonly Ecosystem Ecosystem;
        protected readonly Keycosystem Keycosystem;
        protected readonly Director Director;
        protected readonly DebugLogger DebugLogger;
        protected readonly DirectiveRenderer Renderer;

        protected CameraEntity Camera;

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
            CameraEntity camera,
            DirectiveRenderer renderer)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.Director = director;
            this.DebugLogger = debugLogger;
            this.Camera = camera;
            this.Renderer = renderer;

            // this.Renderer.BackgroundColor = Color.Black;
        }

        public void Load()
        {
            var viewTopLeft = this.Camera.Space.TopLeft;

            this.Ecosystem.AddEntity("Camera", this.Camera);

            // All of the below is some really hacked together proto-UI garbage
            // There will be a better way to do this in the future
            var title = new Entity(new Rect2d(viewTopLeft + new Vector2d(10, 10), Size2d.Zero)); // Text doesn't use size yet
            title.AddComponent(new TextComponent { Text = "Select a scene:", FontAssetName = "Assets/Zelda12" });
            title.AddComponent<DrawComponent>();
            this.Ecosystem.AddEntity("Title", title);

            var cursor = new Entity(new Rect2d(0, 0, 7, 11, Anchor2d.MiddleRight));
            cursor.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Glyphs, Index = 1 });
            cursor.AddComponent<DrawComponent>();
            this.Ecosystem.AddEntity("Cursor", cursor);

            int i = 0;
            foreach (var sceneName in this._selectableScenes)
            {
                var option = new Entity(new Rect2d(title.Space.TopLeft + new Vector2d(20, ++i * 30), Size2d.Zero, Anchor2d.MiddleLeft));
                option.AddComponent(new TextComponent { Text = sceneName, FontAssetName = "Assets/Zelda12" });

                var selectable = new SelectableItemComponent();
                selectable.OnFocus = () => cursor.Space.Position = option.Space.Position + new Vector2d(-5, 6); // text alignment is broken af right now
                selectable.OnBlur = () => { /* Nothing right now! */ };
                option.AddComponent(selectable);
                option.AddComponent<DrawComponent>();

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
                .AddHandler<SpriteHandler>()
                .AddHandler(new DrawHandler(this.Renderer));

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
                });

            this.Keycosystem.AddControlScheme("SceneSelect", controls);
            this.Keycosystem.AddControlScheme("DebugNav", ControlSchemes.DebugNavigation(this.DebugLogger, k));
            this.DebugLogger.AddControlInfo(this.Keycosystem);
        }

        public void Unload()
        {
            this.Keycosystem.ClearControlSchemes();
        }

        public void Update()
        {
            this.Ecosystem.Update();
            this.Keycosystem.Update();
        }

        public void Draw()
        {
            this.Ecosystem.Draw();
            this.DebugLogger.Render();
        }
    }
}
