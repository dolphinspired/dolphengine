using System;
using System.Collections.Generic;
using System.Linq;

namespace DolphEngine.Input
{
    public class Keycosystem
    {
        #region Private properties, indexes

        public readonly GameTimer Timer;
        public readonly KeyStateObserver Observer;

        private readonly Dictionary<string, ControlBase> _controllers = new Dictionary<string, ControlBase>();
        private readonly Dictionary<string, ControlScheme> _controlSchemes = new Dictionary<string, ControlScheme>();

        #endregion

        #region Constructors

        public Keycosystem(GameTimer timer, KeyStateObserver observer)
        {
            this.Timer = timer;
            this.Observer = observer;
        }

        #endregion

        #region Update

        public void Update()
        {
            // First, update the state of the observer (i.e. "initialize" it for this frame)
            this.Observer.Update();

            // Update each controller just once
            foreach (var controller in this._controllers.Select(x => x.Value))
            {
                controller.Update();
            }

            // Finally, run all reactions for each enabled control scheme in the order that they were added
            foreach (var controlScheme in this._controlSchemes.Select(x => x.Value))
            {
                if (!controlScheme.Enabled)
                {
                    continue;
                }

                foreach (var reaction in controlScheme.Reactions)
                {
                    if (reaction.Condition())
                    {
                        reaction.Reaction();
                    }
                }
            }
        }

        #endregion
        
        #region Controllers

        public Keycosystem AddController<TController>(int playerNum, TController controller)
            where TController : ControlBase
        {
            if (playerNum < 1)
            {
                throw new ArgumentException("Player number must be >= 1!");
            }

            var controllerKey = GetControllerKey(playerNum, typeof(TController));
            if (this._controllers.ContainsKey(controllerKey))
            {
                throw new InvalidOperationException($"Player {playerNum} already has a controller of type {typeof(TController).Name}!");
            }

            this._controllers.Add(controllerKey, controller);
            this.Observer.Watch(controller);
            controller.Connect(this);
            return this;
        }

        public TController GetController<TController>(int playerNum)
            where TController : ControlBase
        {
            if (playerNum < 1)
            {
                throw new ArgumentException("Player number must be >= 1!");
            }

            var controllerKey = GetControllerKey(playerNum, typeof(TController));
            if (!this._controllers.TryGetValue(controllerKey, out var controller))
            {
                throw new InvalidOperationException($"Player {playerNum} does not have a controller of type {typeof(TController).Name}!");
            }

            return (TController)controller;
        }

        public Keycosystem RemoveController<TController>(int playerNum)
            where TController : ControlBase
        {
            if (playerNum < 1)
            {
                throw new ArgumentException("Player number must be >= 1!");
            }

            var key = GetControllerKey(playerNum, typeof(TController));
            if (this._controllers.TryGetValue(key, out var controller))
            {
                this._controllers.Remove(key);
                this.Observer.Unwatch(controller);
                // todo: Disconnect controller from keycosystem?
            }

            return this;
        }

        public Keycosystem RemovePlayer(int playerNum)
        {
            if (playerNum < 1)
            {
                throw new ArgumentException("Player number must be >= 1!");
            }

            var partialKey = playerNum + ":";
            foreach (var key in this._controllers.Select(kvp => kvp.Key).Where(k => k.StartsWith(partialKey)))
            {
                this._controllers.Remove(key);
                // todo: Disconnect controller from keycosystem?
            }

            return this;
        }

        public Keycosystem ClearControllers()
        {
            this._controllers.Clear();
            return this;
        }

        #endregion

        #region Control scheme management (core)

        public Keycosystem AddControlScheme(string name, ControlScheme scheme)
        {
            if (this._controlSchemes.ContainsKey(name))
            {
                throw new InvalidOperationException($"A control scheme with name '{name}' has already been added!");
            }

            this._controlSchemes.Add(name, scheme);
            return this;
        }

        public Keycosystem EnableControlScheme(string name)
        {
            if (!this._controlSchemes.TryGetValue(name, out var controlScheme))
            {
                throw new InvalidOperationException($"No control scheme has been added by name '{name}'!");
            }

            controlScheme.Enabled = true;
            return this;
        }

        public Keycosystem DisableControlScheme(string name)
        {
            if (!this._controlSchemes.TryGetValue(name, out var controlScheme))
            {
                throw new InvalidOperationException($"No control scheme has been added by name '{name}'!");
            }

            controlScheme.Enabled = false;
            return this;
        }

        public Keycosystem RemoveControlScheme(string name)
        {
            this._controlSchemes.Remove(name);
            return this;
        }

        public Keycosystem ClearControlSchemes()
        {
            this._controlSchemes.Clear();
            return this;
        }

        #endregion

        #region Non-public stuff

        private static string GetControllerKey(int playerNum, Type type)
        {
            return $"P{playerNum}:{type.FullName}";
        }

        #endregion
    }
}
