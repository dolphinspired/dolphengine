using System;
using System.Collections.Generic;

namespace DolphEngine.DI
{
    public class ServiceRepository : IServiceRepository
    {
        protected readonly Dictionary<Type, Func<object>> Services = new Dictionary<Type, Func<object>>();

        public void AddService(Type type, Func<object> serviceBuilder)
        {
            if (this.Services.ContainsKey(type))
            {
                throw new InvalidOperationException($"Service '{type.Name} has already been registered!");
            }

            this.Services.Add(type, () => serviceBuilder());
        }

        public object GetService(Type type)
        {
            if (!this.Services.TryGetValue(type, out var serviceBuilder))
            {
                throw new InvalidOperationException($"No service has been registered for type '{type.Name}'!");
            }

            return serviceBuilder();
        }
    }
}
