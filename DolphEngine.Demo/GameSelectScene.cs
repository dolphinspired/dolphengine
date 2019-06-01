using DolphEngine.Demo.Components;
using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using DolphEngine.Eco.Entities;
using DolphEngine.Eco.Handlers;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.MonoGame;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DolphEngine.Demo
{
    public class GameSelectScene : IScene
    {
        protected readonly Ecosystem Ecosystem;
        protected readonly Keycosystem Keycosystem;
        protected readonly Director Director;
        protected readonly DebugLogger DebugLogger;

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

        public GameSelectScene(
            Ecosystem ecosystem, 
            Keycosystem keycosystem,
            ContentManager content, 
            SpriteBatch spriteBatch,
            GraphicsDeviceManager gdm,
            Director director,
            DebugLogger debugLogger)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.Director = director;
            this.DebugLogger = debugLogger;

            this.Content = content;
            this.SpriteBatch = spriteBatch;

            this._sceneViewWidth = gdm.PreferredBackBufferWidth;
            this._sceneViewHeight = gdm.PreferredBackBufferHeight;
        }

        public void Load()
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
