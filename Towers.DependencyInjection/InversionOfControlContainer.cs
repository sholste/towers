using System;
using System.Collections.Generic;
using System.Reflection;

namespace Towers.DependencyInjection
{
    public abstract class InversionOfControlContainer
    {
        public const string NO_CONSTRUCTOR_MESSAGE = "Only constructable types are supported";

        protected readonly IDictionary<Type, Type> _registeredTypes = new Dictionary<Type, Type>();

        public abstract void Register<T, TImplementation>() where T : class where TImplementation : T;

        public abstract T Resolve<T>() where T : class;

        public bool IsRegistered(Type type)
        {
            return _registeredTypes.ContainsKey(type);
        }

        protected object ConstructType(Type registeredType)
        {
            if (registeredType == null)
                throw new ArgumentNullException("registeredType");

            Type implementation;
            if (!TryGetRegisteredTypeImplementation(registeredType, out implementation))
            {
                throw new ResolutionFailedException(registeredType);
            }

            var implementationInformation = GetImplementationConstructorWithParameters(implementation);
            if(implementationInformation.Parameters.Length == 0)
                return Activator.CreateInstance(implementation);

            List<object> parameters = ConstructParameters(registeredType, implementationInformation.Parameters);
            return implementationInformation.Constructor.Invoke(parameters.ToArray());
        }

        protected virtual List<object> ConstructParameters(Type registeredType, ParameterInfo[] parameters)
        {
            var additionalInformation = string.Format("Failed on constructor parameter in type {0}", registeredType.FullName);
            List<object> results = new List<object>(parameters.Length);
            foreach (ParameterInfo parameterInfo in parameters)
            {
                try
                {
                    results.Add(ConstructType(parameterInfo.ParameterType));
                }
                // It would be helpful to know which type and parameter had issues and why.
                catch(ResolutionFailedException)
                {
                    throw new ResolutionFailedException(parameterInfo.ParameterType, additionalInformation);
                }
                catch (UnsupportedTypeException ex)
                {
                    throw new UnsupportedTypeException(ex.Message, parameterInfo.ParameterType, additionalInformation);
                }
            }

            return results;
        }

        protected virtual ImplementationInfo GetImplementationConstructorWithParameters(Type implementation)
        {
            ConstructorInfo[] constructors = implementation.GetConstructors();
            // GetConstructors() never returns null, just check length.
            if (constructors.Length < 1)
            {
                throw new UnsupportedTypeException(NO_CONSTRUCTOR_MESSAGE, implementation);
            }

            var defaultConstructor = constructors[0];
            return new ImplementationInfo
            {
                Constructor = defaultConstructor,
                // GetParameters() never returns null.
                Parameters = defaultConstructor.GetParameters()
            };
        }

        protected virtual bool HasEmptyContructor(Type type)
        {
            var constructor = type.GetConstructor(new Type[0]);
            return (constructor != null);
        }

        protected virtual bool TryGetRegisteredTypeImplementation(Type type, out Type implementation)
        {
            if (type != null)
            {
                if (_registeredTypes.ContainsKey(type))
                {
                    implementation = _registeredTypes[type];
                    return true;
                }

                // We can still work with parameterless types with constructors.
                if(HasEmptyContructor(type))
                {
                    implementation = type;
                    return true;
                }
            }

            implementation = null;
            return false;
        }

        public class ImplementationInfo
        {
            public ConstructorInfo Constructor { get; set; }

            public ParameterInfo[] Parameters { get; set; }
        }
    }
}
