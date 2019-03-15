﻿using DolphEngine.Eco;
using DolphEngine.Input;
using System;

namespace DolphEngine.Scenery
{
    public abstract class Scene : IScene
    {
        public readonly Ecosystem Ecosystem;
        public readonly Keycosystem Keycosystem;

        protected readonly GameTimer Timer;

        public Scene(GameTimer timer, Ecosystem ecosystem, Keycosystem keycosystem)
        {
            this.Timer = timer;
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
        }

        public abstract void Load();

        public abstract void Unload();

        public virtual void Update()
        {
            this.Keycosystem.Update();
            this.Ecosystem.Update();
        }

        public virtual void Draw()
        {
            this.Ecosystem.Draw();
        }
    }

    public interface IScene
    {
        void Load();

        void Unload();

        void Update();

        void Draw();
    }
}
