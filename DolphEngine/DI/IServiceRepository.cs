using System;

namespace DolphEngine.DI
{
    public interface IServiceRepository : IServiceProvider
    {
        void AddService(Type type, Func<object> serviceBuilder);
    }
}
