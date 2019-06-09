using DolphEngine.DI;
using System;
using System.Collections.Generic;

namespace DolphEngine.Scenery
{
    public class Director : IDisposable
    {
        protected readonly Dictionary<string, Func<IScene>> Scenes = new Dictionary<string, Func<IScene>>();
        private readonly IServiceRepository _serviceRepo;

        public IScene CurrentScene { get; protected set; }
        public string CurrentSceneName { get; protected set; }

        private string _nextScene;
        private bool _unloadScene;
        
        public Director(IServiceRepository repository)
        {
            this._serviceRepo = repository;
            this._serviceRepo.ResetScope();
        }

        public void Update()
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

            this.CurrentScene.Update();
        }

        public void Draw()
        {
            if (this.CurrentScene != null)
            {
                this.CurrentScene.Draw();
            }
        }

        #region Scene Management

        public Director AddScene<TScene>(string name)
            where TScene : IScene
        {
            this.Scenes.Add(name, this._serviceRepo.BuildInjectableService<IScene, TScene>);
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

        #endregion

        #region Non-public methods

        private void LoadNextScene()
        {
            if (!this.Scenes.TryGetValue(this._nextScene, out var sceneBuilder))
            {
                throw new InvalidOperationException($"No scene has been added with name '{this._nextScene}'!");
            }

            this.UnloadCurrentScene();

            this._serviceRepo.ResetScope();
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

        #endregion

        #region Disposal

        // Following the pattern suggested here: https://stackoverflow.com/a/151244

        ~Director()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    try
                    {
                        this.CurrentScene.Unload();
                    }
                    catch
                    {
                        // todo: log error here
                    }

                    this.Scenes.Clear();

                    this.CurrentScene = null;
                    this.CurrentSceneName = null;
                }

                this._disposed = true;
            }
        }

        #endregion
    }
}
