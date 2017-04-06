namespace Towers.DependencyInjection
{
    /// <summary>
    /// This container is intended to provide default registration and resolution of 
    /// class implementations.
    /// </summary>
    public sealed class TransientContainer : InversionOfControlContainer
    {
        public override void Register<TInterface, TImplementation>()
        {
            _registeredTypes[typeof(TInterface)] = typeof(TImplementation);
        }

        /// <returns>A new instance implementation of the given registered type.</returns>
        public override TInterface Resolve<TInterface>()
        {
            return ConstructType(typeof(TInterface)) as TInterface;
        }
    }
}
