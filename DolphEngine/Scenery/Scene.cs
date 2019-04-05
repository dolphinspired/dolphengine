using DolphEngine.DI;
using DolphEngine.Eco;
using DolphEngine.Input;
using System;

namespace DolphEngine.Scenery
{
    public abstract class Scene : IScene, IServiceProvider
    {
        protected readonly IServiceProvider Services;

        protected readonly GameTimer Timer;
        protected readonly Ecosystem Ecosystem;
        protected readonly Keycosystem Keycosystem;

        public Scene(IServiceProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            this.Services = provider;

            this.Timer = this.GetService<GameTimer>();
            this.Ecosystem = this.GetService<Ecosystem>();
            this.Keycosystem = this.GetService<Keycosystem>();
        }

        public abstract void Load();

        public abstract void Unload();

        public virtual void Update()
        {
            this.Keycosystem.Update(this.Timer.Total);
            this.Ecosystem.Update();
        }

        public virtual void Draw()
        {
            this.Ecosystem.Draw();
        }

        public object GetService(Type serviceType)
        {
            return this.Services.GetService(serviceType);
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
