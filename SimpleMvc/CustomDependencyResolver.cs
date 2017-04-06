using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Towers.DependencyInjection;

namespace SimpleMvc
{
    public class CustomDependencyResolver : IDependencyResolver
    {
        public CustomDependencyResolver()
        {
            ResolveMethod = typeof(Dependency).GetMethod("Resolve", new Type[0]);
        }

        private MethodInfo ResolveMethod { get; set; }

        public object GetService(Type serviceType)
        {
            return GetServices(serviceType).FirstOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!Dependency.IsRegistered(serviceType))
                return new List<object>();

            return new List<object> { TryResolve(serviceType) };
        }

        private object TryResolve(Type type)
        {
            try
            {
                var resolve = ResolveMethod.MakeGenericMethod(new[] { type });
                return resolve.Invoke(null, null);
            }
            catch
            {
                return null;
            }
        }
    }
}