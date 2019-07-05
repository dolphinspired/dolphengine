using DolphEngine.Graphics.Directives;
using System.Collections.Generic;

namespace DolphEngine.UI.Containers
{
    public class TextBox : Container
    {
        #region Properties

        public string Text
        {
            get => this._td.Text;
            set => this._td.Text = value;
        }

        public ColorRGBA Color
        {
            get => this._td.Color;
            set => this._td.Color = value;
        }

        public string Font
        {
            get => this._td.FontAssetName;
            set => this._td.FontAssetName = value;
        }

        #endregion

        #region Container implementation

        private readonly TextDirective _td = new TextDirective
        {
            Color = new ColorRGBA(255, 255, 255)
        };

        public override IEnumerable<DrawDirective> Directives
        {
            get
            {
                this._td.Destination = this.Space.GetOriginPosition();
                yield return this._td;
            }
        }

        #endregion
    }
}
