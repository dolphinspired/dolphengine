using DolphEngine.Graphics.Directives;
using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics
{
    public abstract class DirectiveRenderer
    {
        private readonly Dictionary<Type, Action<DrawDirective>> _renderers = new Dictionary<Type, Action<DrawDirective>>();

        #region Core methods

        public DirectiveRenderer AddRenderer<TDirective>(Action<TDirective> handler)
            where TDirective : DrawDirective
        {
            this._renderers.Add(typeof(TDirective), dir => handler((TDirective)dir));
            return this;
        }

        public void Draw(IEnumerable<DrawDirective> directives)
        {
            if (this._renderers.Count == 0)
            {
                throw new InvalidOperationException($"Cannot draw, no renderers have been added!");
            }

            if (!this.OnBeforeDraw())
            {
                return;
            }

            foreach (var directive in directives)
            {
                if (this._renderers.TryGetValue(directive.GetType(), out var action))
                {
                    action(directive);
                }
            }

            this.OnAfterDraw();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// This method will be called once, before any directives handled.
        /// </summary>
        /// <returns>True to proceed with drawing, false otherwise. If false, renderers and OnAfterDraw will not be called.</returns>
        public abstract bool OnBeforeDraw();

        /// <summary>
        /// This method will be called once, after all directives have been handled.
        /// </summary>
        public abstract void OnAfterDraw();

        #endregion
    }
}
