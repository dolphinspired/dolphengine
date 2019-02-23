using DolphEngine.Eco;
using DolphEngine.Input;
using System;

namespace DolphEngine.Scenery
{
    public abstract class Scene : IScene
    {
        public readonly Ecosystem Ecosystem;
        public readonly Keycosystem Keycosystem;

        private TimeSpan CurrentGameTime;
        protected readonly Func<long> GameTimer;

        public Scene(Ecosystem ecosystem, Keycosystem keycosystem)
        {
            this.Ecosystem = ecosystem;
            this.Keycosystem = keycosystem;
            this.GameTimer = () => this.CurrentGameTime.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public abstract void Load();

        public abstract void Unload();

        public virtual void Update(TimeSpan totalGameTime)
        {
            this.CurrentGameTime = totalGameTime;
            this.Keycosystem.Update(this.GameTimer());
            this.Ecosystem.Update();
        }

        public virtual void Draw(TimeSpan totalGameTime)
        {
            this.Ecosystem.Draw();
        }
    }

    public interface IScene
    {
        void Load();

        void Unload();

        void Update(TimeSpan totalGameTime);

        void Draw(TimeSpan totalGameTime);
    }
}
