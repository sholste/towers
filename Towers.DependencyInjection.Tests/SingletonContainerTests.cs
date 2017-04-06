using Xunit;

namespace Towers.DependencyInjection.Tests
{
    public class SingletonContainerTests
    {
        #region Register Tests

        [Fact]
        public void Register_()
        {
            // Arrange.
            var container = new SingletonContainer();

            // Act, Assert.
            container.Register<IMock, Mock>();
        }

        #endregion

        #region Resolve Tests

        [Fact]
        public void Resolve_RegisteredType_ReturnsImplementation()
        {
            // Arrange.
            var container = new SingletonContainer();
            container.Register<IMock, Mock>();

            // Act.
            var result = container.Resolve<IMock>();

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<Mock>(result);
        }

        [Fact]
        public void Resolve_RegisteredTypePreviouslyResolved_ReturnsSameInstance()
        {
            // Arrange.
            var container = new SingletonContainer();
            container.Register<IMock, Mock>();
            var first = container.Resolve<IMock>();
            first.Value = "some string";

            // Act.
            var result = container.Resolve<IMock>();

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<Mock>(result);
            Assert.Same(first, result);
            Assert.Equal(first.Value, result.Value);
        }

        [Fact]
        public void Resolve_UnregisteredType_ThrowsResolutionFailedException()
        {
            // Arrange.
            var container = new SingletonContainer();

            // Act, Assert.
            Assert.Throws<ResolutionFailedException>(() => {
                container.Resolve<IMock>();
            });
        }

        #endregion

        #region Mocks

        interface IMock
        {
            string Value { get; set; }
        }

        class Mock : IMock
        {
            public string Value { get; set; }
        }

        #endregion
    }
}
