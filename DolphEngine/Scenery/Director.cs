using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Scenery
{
    public class Director : IDisposable
    {
        protected readonly Dictionary<string, IScene> Scenes = new Dictionary<string, IScene>();

        public IScene CurrentScene { get; protected set; }
        public string CurrentSceneName { get; protected set; }

        ~Director()
        {
            this.Dispose();
        }

        public Director AddScene(string name, IScene scene)
        {
            this.Scenes.Add(name, scene);
            return this;
        }

        public Director RemoveScene(string name)
        {
            this.Scenes.Remove(name);
            return this;
        }

        public virtual Director LoadScene(string name)
        {
            if (!this.Scenes.TryGetValue(name, out var scene))
            {
                throw new ArgumentException($"No scene has been added with name {name}!");
            }

            if (this.CurrentScene != null)
            {
                this.CurrentScene.Unload();
            }

            this.CurrentScene = scene;
            this.CurrentSceneName = name;

            scene.Load();
            return this;
        }

        public virtual Director UnloadScene()
        {
            if (this.CurrentScene != null)
            {
                this.CurrentScene.Unload();
                this.CurrentScene = null;
                this.CurrentSceneName = null;
            }

            return this;
        }

        public void Dispose()
        {
            foreach (var scene in this.Scenes.Select(x => x.Value))
            {
                scene.Unload();
            }

            this.CurrentScene = null;
            this.CurrentSceneName = null;
        }
    }
}
