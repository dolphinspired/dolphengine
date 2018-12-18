using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Demo
{
    public class DebugLogger
    {
        public static readonly Func<string> EmptyLine = () => "";

        public bool Hidden;
        public int CurrentPage;

        public SpriteFont Font;
        public Color FontColor = Color.White;
        public int FontSize = 10;
        public int PaddingTop = 12;
        public int PaddingLeft = 12;
        public int LineSpacing = 4;

        private readonly Dictionary<int, List<Func<string>>> _pages = new Dictionary<int, List<Func<string>>>();

        public void NextPage()
        {
            var next = this.CurrentPage + 1;

            if (!this._pages.ContainsKey(next))
            {
                next = this._pages.First().Key;
            }

            this.CurrentPage = next;
        }

        public void PrevPage()
        {
            var prev = this.CurrentPage - 1;

            if (!this._pages.ContainsKey(prev))
            {
                prev = this._pages.Last().Key;
            }

            this.CurrentPage = prev;
        }

        public void AddLine(int page)
        {
            this.AddLine(1, () => "");
        }

        public void AddLine(int page, string line)
        {
            this.AddLine(page, () => line);
        }

        public void AddLine(int page, params Func<string>[] lines)
        {
            foreach (var line in lines)
            {
                if (this._pages.ContainsKey(page))
                {
                    this._pages[page].Add(line);
                }
                else
                {
                    this._pages.Add(page, new List<Func<string>> { line });
                }
            }
        }

        public void Render(SpriteBatch sb)
        {
            if (this.Hidden)
            {
                return;
            }

            var pos = new Vector2(this.PaddingLeft, this.PaddingTop);

            foreach (var line in this._pages[this.CurrentPage])
            {
                sb.DrawString(this.Font, line(), pos, this.FontColor);
                pos.Y += this.FontSize + this.LineSpacing;
            }
        }
    }
}
