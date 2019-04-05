using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Scenery
{
    public class Director : IDisposable
    {
        protected readonly Dictionary<string, Func<IScene>> Scenes = new Dictionary<string, Func<IScene>>();
        protected readonly Dictionary<Type, Func<object>> Services = new Dictionary<Type, Func<object>>();

        public IScene CurrentScene { get; protected set; }
        public string CurrentSceneName { get; protected set; }

        private string _nextScene;
        private bool _unloadScene;

        ~Director()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            foreach (var sceneBuilder in this.Scenes.Select(x => x.Value))
            {
                try
                {
                    sceneBuilder().Unload();
                }
                catch
                {
                }
            }

            this.Scenes.Clear();
            this.Services.Clear();

            this.CurrentScene = null;
            this.CurrentSceneName = null;
        }

        #region Dependency Injection

        public Director AddService<TService>()
        {
            return this.AddService<TService, TService>();
        }

        public Director AddService<TService, TImplementation>()
            where TImplementation : TService
        {
            return this.AddService(this.BuildInjectableService<TService, TImplementation>);
        }

        public Director AddService<TService>(TService service)
        {
            return this.AddService(() => service);
        }

        public Director AddService<TService>(Func<TService> serviceBuilder)
        {
            if (this.Services.ContainsKey(typeof(TService)))
            {
                throw new InvalidOperationException($"Service '{typeof(TService).Name} has already been registered!");
            }

            this.Services.Add(typeof(TService), () => serviceBuilder());
            return this;
        }

        public TService GetService<TService>()
        {
            var type = typeof(TService);
            if (!this.Services.TryGetValue(type, out var serviceBuilder))
            {
                throw new InvalidOperationException($"No service has been registered for type '{typeof(TService)}'!");
            }

            return (TService)serviceBuilder();
        }

        #endregion

        #region Scene Management

        public Director AddScene<TScene>(string name)
            where TScene : IScene
        {
            this.Scenes.Add(name, this.BuildInjectableService<IScene, TScene>);
            return this;
        }

        public Director AddScene(string name, IScene scene)
        {
            this.Scenes.Add(name, () => scene);
            return this;
        }

        public Director RemoveScene(string name)
        {
            this.Scenes.Remove(name);
            return this;
        }

        public Director LoadScene(string name)
        {
            // The new scene will be loaded before the next update
            this._nextScene = name;
            return this;
        }

        public Director UnloadScene()
        {
            // The scene will be unloaded before the next update
            this._unloadScene = true;
            return this;
        }

        public void Update(TimeSpan time)
        {
            if (this._unloadScene)
            {
                this.UnloadCurrentScene();
                this._unloadScene = false;
            }
            else if (this._nextScene != null)
            {
                this.LoadNextScene();
                this._nextScene = null;
            }

            this.CurrentScene.Update(time);
        }

        #endregion

        #region Non-public methods

        private void LoadNextScene()
        {
            if (!this.Scenes.TryGetValue(this._nextScene, out var sceneBuilder))
            {
                throw new InvalidOperationException($"No scene has been added with name '{this._nextScene}'!");
            }

            this.UnloadCurrentScene();

            this.CurrentScene = sceneBuilder();
            this.CurrentSceneName = this._nextScene;
            this.CurrentScene.Load();
        }

        private void UnloadCurrentScene()
        {
            if (this.CurrentScene != null)
            {
                this.CurrentScene.Unload();
                this.CurrentScene = null;
                this.CurrentSceneName = null;
            }
        }

        // Adapted from: https://rlbisbe.net/2014/08/04/creating-a-dependency-injection-engine-with-c/
        private TService BuildInjectableService<TService, TImplementation>()
            where TImplementation : TService
        {
            try
            {
                var constructor = typeof(TImplementation).GetConstructors()[0];
                var parameters = constructor.GetParameters();

                var resolvedParameters = new object[parameters.Length];

                var i = 0;
                foreach (var par in parameters)
                {
                    resolvedParameters[i++] = this.Services[par.ParameterType]();
                }

                return (TService)constructor.Invoke(resolvedParameters);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Dependency injection failed for service '{typeof(TService).Name}' with implementation '{typeof(TImplementation).Name}'!", e);
            }
        }

        #endregion
    }
}
