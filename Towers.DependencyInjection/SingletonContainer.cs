using System;
using System.Collections.Generic;

namespace Towers.DependencyInjection
{
    /// <summary>
    /// This container is intended to provide registration and resolution of 
    /// singleton class implementations.
    /// </summary>
    public sealed class SingletonContainer : InversionOfControlContainer
    {
        private readonly IDictionary<Type, object> _instances = new Dictionary<Type, object>();

        public override void Register<TInterface, TImplementation>()
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        /// <returns>
        /// An existing class instance if previously resolved or
        /// a new class instance to be used as a singleton by future consumers.
        /// TODO: Comment make sense?
        /// </returns>
        public override TInterface Resolve<TInterface>()
        {
            var type = typeof(TInterface);
            Type implementation;

            // First try and find an existing instance and use that.
            var existingInstance = FindSingletonInstance(type, out implementation);
            if(existingInstance != null)
                return existingInstance as TInterface;

            // If not found return a new instance and cache.
            var instance = ConstructType(type);
            if(implementation != null)
                _instances.Add(implementation, instance);

            return instance as TInterface;
        }

        private object FindSingletonInstance(Type registeredType, out Type implementation)
        {
            implementation = null;
            if (_registeredTypes.ContainsKey(registeredType))
            {
                implementation = _registeredTypes[registeredType];
                if (_instances.ContainsKey(implementation))
                    return _instances[implementation];
            }
            
            return null;
        }
    }
}
