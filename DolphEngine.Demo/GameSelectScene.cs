using DolphEngine.Demo.Components;
using DolphEngine.DI;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Input;
using DolphEngine.MonoGame.Graphics;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.Demo
{
    public class GameSelectScene : Scene
    {
        protected readonly ContentManager Content;
        protected readonly SpriteBatch SpriteBatch;

        protected CameraEntity Camera;

        private readonly int _sceneViewWidth;
        private readonly int _sceneViewHeight;

        private readonly string[] _selectableScenes = new[]
        {
            Scenes.TestMapScene,
            Scenes.DogTreasureHunt,
            "not a real scene",
            "just padding the list length",
        };

        private int _selectedIndex;
        private readonly List<Entity> _selectableEntities = new List<Entity>();

        public GameSelectScene(IServiceProvider services) : base(services)
        {
            this.Content = this.GetService<ContentManager>();
            this.SpriteBatch = this.GetService<SpriteBatch>();

            var gdm = this.GetService<GraphicsDeviceManager>();
            this._sceneViewWidth = gdm.PreferredBackBufferWidth;
            this._sceneViewHeight = gdm.PreferredBackBufferHeight;
        }

        public override void Load()
        {
            this.Camera = new CameraEntity(this._sceneViewWidth, this._sceneViewHeight);
            var viewTopLeft = this.Camera.Space.TopLeft;

            var entities = new List<Entity> { this.Camera };

            // All of the below is some really hacked together proto-UI garbage
            // There will be a better way to do this in the future
            var title = new Entity("Title");
            title.Space = new Rect2d(viewTopLeft + new Vector2d(10, 10), Size2d.Zero); // Text doesn't use size yet
            title.AddComponent(new TextComponent { Text = "Select a scene:", FontAssetName = "Debug" });
            title.AddComponent<DrawComponent>();
            entities.Add(title);

            var cursor = new Entity("Cursor");
            cursor.Space = new Rect2d(0, 0, 7, 11, Anchor2d.MiddleRight);
            cursor.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Glyphs, Index = 1 });
            cursor.AddComponent<DrawComponent>();
            entities.Add(cursor);

            int i = 0;
            foreach (var sceneName in this._selectableScenes)
            {
                var option = new Entity($"scene:{sceneName}");
                option.Space = new Rect2d(title.Space.TopLeft + new Vector2d(20, ++i * 30), Size2d.Zero, Anchor2d.MiddleLeft);
                option.AddComponent(new TextComponent { Text = sceneName, FontAssetName = "Debug" });

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

                entities.Add(option);
                this._selectableEntities.Add(option);
            }

            this.Ecosystem.AddEntities(entities);

            var renderer = new MonoGameRenderer(this.SpriteBatch, this.Content, this.Camera);
            renderer.BackgroundColor = Color.Black;

            this.Ecosystem
                .AddHandler<TextHandler>()
                .AddHandler<SpriteHandler>()
                .AddHandler(new DrawHandler(renderer));

            var keyContext = new KeyContext("SceneSelect")
                .AddControl(Tower.Keyboard, k => k.Down.JustPressed, k =>
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
                .AddControl(Tower.Keyboard, k => k.Up.JustPressed, k =>
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
                .AddControl(Tower.Keyboard, k => k.Enter.JustPressed, k =>
                {
                    var selectedSceneName = this._selectableScenes[this._selectedIndex];
                    Tower.Director.LoadScene(selectedSceneName);
                });

            this.Keycosystem.AddContext(keyContext);
        }

        public override void Unload()
        {
            this.Keycosystem.RemoveContext("SceneSelect");
        }
    }
}
