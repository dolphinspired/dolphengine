using DolphEngine.Eco;
using DolphEngine.Eco.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace DolphEngine.Demo.Games.DogGame.Entities
{
    public class DogEntity : Entity
    {
        public DogEntity()
        {
            this.AddComponent(new SpriteComponent { SpriteSheet = Sprites.DogStuff, Index = 0 });
        }
    }
}
