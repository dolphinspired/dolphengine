﻿using DolphEngine.DI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Scenery
{
    public class Director : IServiceRepository, IDisposable
    {
        protected readonly Dictionary<string, Func<IScene>> Scenes = new Dictionary<string, Func<IScene>>();
        private readonly IServiceRepository _serviceRepo;

        public IScene CurrentScene { get; protected set; }
        public string CurrentSceneName { get; protected set; }

        private string _nextScene;
        private bool _unloadScene;

        public Director()
        {
            this._serviceRepo = new ServiceRepository();
        }

        public Director(IServiceRepository repository)
        {
            this._serviceRepo = repository;
        }

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

            this.CurrentScene = null;
            this.CurrentSceneName = null;
        }

        #region Dependency Injection

        public object GetService(Type serviceType)
        {
            return this._serviceRepo.GetService(serviceType);
        }

        public void AddService(Type type, Func<object> serviceBuilder)
        {
            this._serviceRepo.AddService(type, serviceBuilder);
        }

        #endregion

        #region Scene Management

        public Director AddScene<TScene>(string name)
            where TScene : Scene
        {
            this.Scenes.Add(name, () => (TScene)Activator.CreateInstance(typeof(TScene), this._serviceRepo));
            return this;
        }

        public Director AddScene(string name, Func<IScene> sceneBuilder)
        {
            this.Scenes.Add(name, sceneBuilder);
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

        #endregion
    }
}
