using System;
using DependencyInjectorBenchmarks.Scenarios;

namespace DependencyInjectorBenchmarks.Containers
{
    public class StaticGenericPollutionBenchmark : IContainerBenchmark
    {
        public static readonly StaticGenericPollutionBenchmark Instance = new StaticGenericPollutionBenchmark();

        public StaticGenericPollutionBenchmark()
        {
            // constructing the factories in register is an exercise for the reader
            Resolver.Register<ISingleton>(() => Singleton.Instance);
            Resolver.Register<ITransient>(() => new Transient());
            Resolver.Register<ICombined>(() => new Combined(Resolver.Resolve<ISingleton>(), Resolver.Resolve<ITransient>()));
        }

        public ICombined ResolveCombined() => Resolver.Resolve<ICombined>();

        public ISingleton ResolveSingleton() => Resolver.Resolve<ISingleton>();

        public ITransient ResolveTransient() => Resolver.Resolve<ITransient>();

        public static class Resolver
        {
            public static void Register<T>(Func<T> factory)
            {
                Lookup<T>.Resolve = factory;
            }

            public static T Resolve<T>()
            {
                var factory = Lookup<T>.Resolve;
                if (factory != null)
                {
                    return factory();
                }

                return default(T);
            }

            protected static class Lookup<T>
            {
                public static Func<T> Resolve { get; set; }
            }
        }
    }
}
