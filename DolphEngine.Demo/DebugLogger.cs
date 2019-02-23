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

        private readonly List<List<Func<string>>> _pages = new List<List<Func<string>>>();

        public int NextPage()
        {
            if (this._pages.Count == 0)
            {
                this.CurrentPage = 0;
                return 0;
            }

            var next = this.CurrentPage + 1;

            if (next >= this._pages.Count)
            {
                next = 0;
            }

            this.CurrentPage = next;
            return next;
        }

        public int PrevPage()
        {
            if (this._pages.Count == 0)
            {
                this.CurrentPage = 0;
                return 0;
            }

            var prev = this.CurrentPage - 1;

            if (prev < 0)
            {
                prev = this._pages.Count - 1;
            }

            this.CurrentPage = prev;
            return prev;
        }

        public void AddLine(int page)
        {
            this.AddLine(page, EmptyLine);
        }

        public void AddLine(int page, string line)
        {
            this.AddLine(page, () => line);
        }

        public void AddLine(int page, params Func<string>[] lines)
        {
            if (page < this._pages.Count)
            {
                this._pages[page].AddRange(lines);
            }
        }

        public int AddPage(params Func<string>[] lines)
        {
            this._pages.Add(lines?.ToList() ?? new List<Func<string>>(0));
            return this._pages.Count - 1;
        }

        public void Render(SpriteBatch sb)
        {
            if (this.Hidden || this._pages.Count == 0 || this.CurrentPage >= this._pages.Count || this.CurrentPage < 0)
            {
                return;
            }

            var pos = new Vector2(this.PaddingLeft, this.PaddingTop);

            sb.Begin();

            foreach (var line in this._pages[this.CurrentPage])
            {
                sb.DrawString(this.Font, line(), pos, this.FontColor);
                pos.Y += this.FontSize + this.LineSpacing;
            }

            sb.End();
        }
    }
}
