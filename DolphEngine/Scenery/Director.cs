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

        public virtual Director LoadScene(string name)
        {
            if (!this.Scenes.TryGetValue(name, out var sceneBuilder))
            {
                throw new ArgumentException($"No scene has been added with name {name}!");
            }

            if (this.CurrentScene != null)
            {
                this.CurrentScene.Unload();
            }

            this.CurrentScene = sceneBuilder();
            this.CurrentSceneName = name;

            this.CurrentScene.Load();
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

        #endregion

        #region Non-public methods

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
