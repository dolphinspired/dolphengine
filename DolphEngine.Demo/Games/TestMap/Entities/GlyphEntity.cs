﻿using DolphEngine.Eco;
using DolphEngine.Eco.Components;

namespace DolphEngine.Demo.Games.TestMap.Entities
{
    public class GlyphEntity : Entity
    {
        public GlyphEntity(int glyphIndex, string name) : base(name)
        {
            this.AddComponent(new SpriteComponent { SpriteSheet = Sprites.Glyphs, Index = glyphIndex })
                .AddComponent<DrawComponent>();
        }

        public SpriteComponent Sprite => this.GetComponent<SpriteComponent>();
    }
}