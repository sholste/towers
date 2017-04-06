using System;
using Xunit;

namespace Towers.DependencyInjection.Tests
{
    public class DependencyTests
    {
        #region Register Tests

        [Fact]
        public void Register_Called_NoException()
        {
            // Arrange, Act, Assert.
            Dependency.Register<IMock, Mock>();
        }

        #endregion

        #region Resolve Tests

        [Fact]
        public void Resolve_RegisteredType_ReturnsImplementation()
        {
            // Arrange.
            Dependency.Reset();
            Dependency.Register<IMock, Mock>();

            // Act.
            var result = Dependency.Resolve<IMock>();

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<Mock>(result);
        }

        #endregion

        #region Singleton.Register Tests

        [Fact]
        public void SingletonRegister_Called_NoException()
        {
            // Arrange, Act, Assert.
            Dependency.Singleton.Register<IMock, Mock>();
        }

        #endregion

        #region Singleton.Resolve Tests

        [Fact]
        public void SingletonResolve_RegisteredType_ReturnsImplementation()
        {
            // Arrange.
            Dependency.Singleton.Reset();
            Dependency.Singleton.Register<IMock, Mock>();

            // Act.
            var result = Dependency.Singleton.Resolve<IMock>();

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<Mock>(result);
        }

        #endregion

        #region Mocks

        interface IMock
        {
            string Value { get; set; }
        }

        class Mock: IMock
        {
            public string Value { get; set; }
        }

        #endregion
    }
}
