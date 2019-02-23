//using Microsoft.Xna.Framework;

namespace DolphEngine.Old
{
    public class Highlight
    {
        //public static Highlight None => new Highlight();

        //public int InlineWidth;

        //public Color? InlineColor;

        //public int OutlineWidth;

        //public Color? OutlineColor;

        //public Color? FillColor;

        //public bool IsVisible =>
        //    (this.InlineColor.HasValue && this.InlineWidth > 0) ||
        //    (this.OutlineColor.HasValue && this.OutlineWidth > 0) ||
        //    this.FillColor.HasValue;
    }

    // todo: for spritesheets, might be better to build a highlight texture for the whole sheet and cache it
    //private Texture2D BuildHighlightTexture(Texture2D spriteTexture, Rectangle src, BoxHighlightComponent highlight)
    //{
    //    var wholeTextureColorData = new Color[spriteTexture.Width * spriteTexture.Height];
    //    spriteTexture.GetData(wholeTextureColorData);

    //    var activeTexture = new Texture2D(spriteTexture.GraphicsDevice, src.Width, src.Height);
    //    var activeColorData = new Color[src.Width * src.Height];
    //    for (int x = 0; x < src.Width; x++)
    //    {
    //        for (int y = 0; y < src.Height; y++)
    //        {
    //            // Only copy over the rect of the sprite that we are actively drawing
    //            activeColorData[y * src.Width + x] = wholeTextureColorData[(y + src.Y) * spriteTexture.Width + (x + src.X)];
    //        }
    //    }

    //    var highlightColorData = new Color[src.Width * src.Height];

    //    if (highlight.FillColor != null)
    //    {
    //        // Adapted from Olander, found in this thread: http://community.monogame.net/t/how-to-make-outline-glow-on-sprites-2d-pixel-game/8059/2
    //        for (int x = 0; x < src.Width; x++)
    //        {
    //            for (int y = 0; y < src.Height; y++)
    //            {
    //                var srcAlpha = (float)activeColorData[y * src.Width + x].A / 255f;
    //                var dstAlpha = highlight.FillColor.Value.A;

    //                highlightColorData[y * src.Width + x] = srcAlpha > 0
    //                    ? highlight.FillColor.Value * (srcAlpha + dstAlpha)
    //                    : Color.Transparent;
    //            }
    //        }
    //    }

    //    if (highlight.OutlineColor != null && highlight.OutlineWidth > 0)
    //    {

    //    }

    //    if (highlight.InlineColor != null && highlight.InlineWidth > 0)
    //    {

    //    }

    //    var highlightTexture = new Texture2D(spriteTexture.GraphicsDevice, src.Width, src.Height);
    //    highlightTexture.SetData(highlightColorData);
    //    return highlightTexture;
    //}
}
