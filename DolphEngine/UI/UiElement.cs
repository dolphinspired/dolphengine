using DolphEngine.Graphics;
using DolphEngine.Graphics.Directives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.UI
{
    public abstract class UiElement : Rect2dBase, IDirectiveChannel
    {
        #region Core fields

        private Type _type;
        public Type Type => this._type ?? (this._type = this.GetType());

        private bool _isChanged = true; // All elements start out as "changed" when they are created
        public bool IsChanged
        {
            get => this._isChanged;
            protected set
            {
                if (value && !this.IsChanged && this.Parent != null)
                {
                    // On first change, mark parent changed as well
                    // This will propagate up the tree all the way back to the Window
                    this.Parent.IsChanged = true;
                }
                this._isChanged = value;
            }
        }

        #endregion

        #region Rect2dBase properties

        public override float X
        {
            set
            {
                base.X = value;
                this.IsChanged = true;
            }
        }

        public override float Y
        {
            set
            {
                base.Y = value;
                this.IsChanged = true;
            }
        }

        public override float Width
        {
            set
            {
                base.Width = value;
                this.IsChanged = true;
            }
        }

        public override float Height
        {
            set
            {
                base.Height = value;
                this.IsChanged = true;
            }
        }

        #endregion

        #region Element tree

        public UiElement Root
        {
            get
            {
                UiElement root = this;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }
                return root;
            }
        }

        private UiElement _parent;
        public UiElement Parent => this._parent;

        private List<UiElement> _children;
        public IReadOnlyCollection<UiElement> Children => this._children;

        public UiElement AddChild(UiElement element)
        {
            if (this._children == null)
            {
                this._children = new List<UiElement>();
            }

            this._children.Add(element);
            element._parent = this;
            this.IsChanged = true;
            return this;
        }

        public UiElement AddChildren(IEnumerable<UiElement> elements)
        {
            if (this._children == null)
            {
                this._children = new List<UiElement>();
            }

            foreach (var element in elements)
            {
                this._children.Add(element);
                element._parent = this;
            }

            this.IsChanged = true;
            return this;
        }

        public UiElement RemoveChild(UiElement element)
        {
            if (this._children != null)
            {
                this._children.Remove(element);
                element._parent = null;
                this.IsChanged = true;
            }
            return this;
        }

        public UiElement RemoveChildren(IEnumerable<UiElement> elements)
        {
            if (this._children != null)
            {
                foreach (var element in elements)
                {
                    this._children.Remove(element);
                    element._parent = null;
                }
                
                this.IsChanged = true;
            }
            return this;
        }

        public UiElement RemoveChildren()
        {
            if (this._children != null)
            {
                foreach (var child in this._children)
                {
                    child._parent = null;
                }

                this._children = null;
                this.IsChanged = true;
            }
            return this;
        }

        #endregion

        #region Directives

        public virtual IEnumerable<DrawDirective> Directives
        {
            get
            {
                yield break;
            }
        }

        private IEnumerable<DrawDirective> _treeDirectives;
        public IEnumerable<DrawDirective> TreeDirectives
        {
            get
            {
                if (this._isChanged)
                {
                    // If there was a change, rebuild the tree from here down, then reset IsChanged until another change is made
                    this._treeDirectives = Enumerable.Concat(this.Directives, this.Children.SelectMany(c => c.TreeDirectives));
                    this._isChanged = false;
                }
                
                return this._treeDirectives;
            }
        }

        #endregion
    }
}
