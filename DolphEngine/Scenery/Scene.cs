using DolphEngine.Eco;
using DolphEngine.Input;
using System;

namespace DolphEngine.Scenery
{
    public abstract class Scene : IScene
    {
        public readonly Ecosystem Ecosystem;
        public readonly Keycosystem Keycosystem;

        private long CurrentGameTicks;
        protected readonly Func<long> GameTimer;

        public Scene(Ecosystem ecosystem, Keycosystem keycosystem)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.GameTimer = () => this.CurrentGameTicks;
        }

        public abstract void Load();

        public abstract void Unload();

        public virtual void Update(long totalGameTicks)
        {
            this.CurrentGameTicks = totalGameTicks;
            this.Keycosystem.Update(totalGameTicks);
            this.Ecosystem.Update();
        }

        public virtual void Draw(long totalGameTicks)
        {
            this.Ecosystem.Draw();
        }
    }

    public interface IScene
    {
        void Load();

        void Unload();

        void Update(long totalGameTicks);

        void Draw(long totalGameTicks);
    }
}
