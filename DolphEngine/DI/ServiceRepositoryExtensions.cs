using System;

namespace DolphEngine.DI
{
    public static class ServiceRepositoryExtensions
    {
        public static IServiceRepository AddService<TService>(this IServiceRepository repository)
        {
            repository.AddService(typeof(TService), () => repository.BuildInjectableService<TService, TService>());
            return repository;
        }

        public static IServiceRepository AddService<TService, TImplementation>(this IServiceRepository repository)
            where TImplementation : TService
        {
            repository.AddService(typeof(TService), () => repository.BuildInjectableService<TService, TImplementation>());
            return repository;
        }

        public static IServiceRepository AddService<TService>(this IServiceRepository repository, TService service)
        {
            repository.AddService(typeof(TService), () => service);
            return repository;
        }

        public static IServiceRepository AddService<TService>(this IServiceRepository repository, Func<TService> serviceBuilder)
        {
            repository.AddService(typeof(TService), () => serviceBuilder());
            return repository;
        }

        public static TService GetService<TService>(this IServiceProvider provider)
        {
            return (TService)provider.GetService(typeof(TService));
        }

        // Adapted from: https://rlbisbe.net/2014/08/04/creating-a-dependency-injection-engine-with-c/
        public static TService BuildInjectableService<TService, TImplementation>(this IServiceProvider provider)
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
                    resolvedParameters[i++] = provider.GetService(par.ParameterType);
                }

                return (TService)constructor.Invoke(resolvedParameters);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Dependency injection failed for service '{typeof(TService).Name}' with implementation '{typeof(TImplementation).Name}'!", e);
            }
        }
    }
}
