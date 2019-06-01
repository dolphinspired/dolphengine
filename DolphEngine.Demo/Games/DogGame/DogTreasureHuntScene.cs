using DolphEngine.Eco;
using DolphEngine.Input;
using DolphEngine.Input.Controllers;
using DolphEngine.Scenery;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolphEngine.Demo.Games.DogGame
{
    public class DogTreasureHuntScene : IScene
    {
        private readonly Ecosystem Ecosystem;
        private readonly Keycosystem Keycosystem;
        private readonly StandardKeyboard Keyboard;

        private readonly SpriteBatch SpriteBatch;
        private readonly ContentManager Content;

        public DogTreasureHuntScene(Ecosystem ecosystem, Keycosystem keycosystem, SpriteBatch spriteBatch, ContentManager content, StandardKeyboard keyboard)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.Keyboard = keyboard;

            this.SpriteBatch = spriteBatch;
            this.Content = content;
        }

        public void Load()
        {
            
        }

        public void Unload()
        {
            
        }

        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}
