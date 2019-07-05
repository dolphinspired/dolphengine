using DolphEngine.Graphics.Directives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Graphics
{
    public abstract class DirectiveRenderer
    {
        private readonly Dictionary<Type, Action<DrawDirective>> _renderers = new Dictionary<Type, Action<DrawDirective>>();
        // todo: this structure isn't great if you want to reference the same viewChannels in multiple panes (i.e. split-screen multiplayer), but it'll do for now
        private readonly Dictionary<Viewport2d, List<IDirectiveChannel>> _viewChannels = new Dictionary<Viewport2d, List<IDirectiveChannel>>(ReferenceEqualityComparer<Viewport2d>.Default);
        private readonly Dictionary<string, Viewport2d> _viewports = new Dictionary<string, Viewport2d>();
        
        #region Core methods

        public DirectiveRenderer AddRenderer<TDirective>(Action<TDirective> handler)
            where TDirective : DrawDirective
        {
            this._renderers.Add(typeof(TDirective), dir => handler((TDirective)dir));
            return this;
        }

        // This might be a candidate for some sort of stream structure
        public DirectiveRenderer AddViewChannel(string viewportName, IDirectiveChannel channel)
        {
            if (!this._viewports.TryGetValue(viewportName, out var viewport))
            {
                throw new InvalidOperationException($"No viewport has been added with name '{viewportName}' - cannot add a channel to it!");
            }

            if (!this._viewChannels.TryGetValue(viewport, out var channels))
            {
                channels = new List<IDirectiveChannel>();
                this._viewChannels.Add(viewport, channels);
            }

            channels.Add(channel);
            return this;
        }

        public DirectiveRenderer AddViewport(string name, Viewport2d viewport)
        {
            if (this._viewports.ContainsKey(name))
            {
                throw new InvalidOperationException($"A viewport has already been added with name '{name}'!");
            }

            this._viewports.Add(name, viewport);
            return this;
        }

        public Viewport2d GetViewport(string name)
        {
            if (!this._viewports.TryGetValue(name, out var viewport))
            {
                throw new InvalidOperationException($"No viewport has been added with name '{name}'!");
            }

            return viewport;
        }

        public void Draw()
        {
            if (this._renderers.Count == 0)
            {
                throw new InvalidOperationException($"Cannot draw, no renderers have been added!");
            }

            if (!this.OnBeforeDraw())
            {
                return;
            }

            foreach (var viewChannel in this._viewChannels)
            {
                var viewport = viewChannel.Key;

                if (!this.OnBeforeRenderView(viewport))
                {
                    continue;
                }

                foreach (var directive in viewChannel.Value.SelectMany(c => c.Directives))
                {
                    if (this._renderers.TryGetValue(directive.GetType(), out var action))
                    {
                        action(directive);
                    }
                }

                this.OnAfterRenderView(viewport);
            }

            this.OnAfterDraw();
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// This method will be called once, before any directives are handled.
        /// </summary>
        /// <returns>True to proceed with drawing, false otherwise. If false, renderers and OnAfterDraw will not be called.</returns>
        public virtual bool OnBeforeDraw()
        {
            return true;
        }

        public virtual bool OnBeforeRenderView(Viewport2d viewport)
        {
            return true;
        }

        public virtual void OnAfterRenderView(Viewport2d viewport)
        {
        }

        /// <summary>
        /// This method will be called once, after all directives have been handled.
        /// </summary>
        public virtual void OnAfterDraw()
        {
        }

        #endregion
    }
}
