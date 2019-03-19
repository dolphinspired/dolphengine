using System;
using System.Collections.Generic;

namespace DolphEngine.Graphics
{
    public abstract class DirectiveRenderer
    {
        private readonly Dictionary<Type, Action<object>> _renderers = new Dictionary<Type, Action<object>>();

        #region Core methods

        public DirectiveRenderer AddRenderer<TDirective>(Action<TDirective> handler)
        {
            this._renderers.Add(typeof(TDirective), new Action<object>(o => handler((TDirective)o)));
            return this;
        }

        public void Draw(IEnumerable<object> directives)
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
