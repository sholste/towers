using System;

namespace Towers.DependencyInjection
{
    /// <summary>
    /// This is to be the wrapper class from registering and resolving
    /// the different lifecycle IoC containers.
    /// </summary>
    public static class Dependency
    {
        private static SingletonContainer _singletonContainer = new SingletonContainer();
        private static TransientContainer _transientContainer = new TransientContainer();

        public static bool IsRegistered(Type type)
        {
            return _transientContainer.IsRegistered(type)
                || _singletonContainer.IsRegistered(type);
        }

        public static void Register<TInterface, TImplementation>() where TInterface : class where TImplementation : TInterface
        {
            _transientContainer.Register<TInterface, TImplementation>();
        }

        public static void Reset()
        {
            _transientContainer = new TransientContainer();
        }

        public static TInterface Resolve<TInterface>() where TInterface : class
        {
            return _transientContainer.Resolve<TInterface>();
        }

        public static class Singleton
        {
            public static void Register<TInterface, TSingleton>() where TInterface : class where TSingleton : TInterface
            {
                _singletonContainer.Register<TInterface, TSingleton>();
            }

            public static void Reset()
            {
                _singletonContainer = new SingletonContainer();
            }

            public static TInterface Resolve<TInterface>() where TInterface : class
            {
                return _singletonContainer.Resolve< TInterface>();
            }
        }

        // -- Wire in new Lifecycle container here. ie ThreadStatic.
    }
}
