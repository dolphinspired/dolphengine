using DolphEngine.Graphics.Directives;
using System.Collections.Generic;

namespace DolphEngine.UI.Containers
{
    public class TextBox : UiElement
    {
        public TextBox()
        {
            this.Color = new ColorRGBA(255, 255, 255);
        }

        #region Properties

        public string Text
        {
            get => this._td.Text;
            set
            {
                this._td.Text = value;
                this.IsChanged = true;
            }

        }

        public ColorRGBA Color
        {
            get => this._td.Color;
            set
            {
                this._td.Color = value;
                this.IsChanged = true;
            }
        }

        public string Font
        {
            get => this._td.FontAssetName;
            set
            {
                this._td.FontAssetName = value;
                this.IsChanged = true;
            }
        }

        #endregion

        #region Container implementation

        private readonly TextDirective _td = new TextDirective();

        public override IEnumerable<DrawDirective> Directives
        {
            get
            {
                this._td.Destination = this.GetOriginPosition();
                yield return this._td;
            }
        }

        #endregion
    }
}
