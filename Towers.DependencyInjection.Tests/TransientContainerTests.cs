using Xunit;

namespace Towers.DependencyInjection.Tests
{
    public class TransientContainerTests
    {
        #region Register Tests

        [Fact]
        public void Register_Called_NoException()
        {
            // Arrange.
            var container = new TransientContainer();

            // Act, Assert.
            container.Register<IMock, Mock>();
        }

        #endregion

        #region Resolve Tests

        [Fact]
        public void Resolve_RegisteredType_ReturnsImplementation()
        {
            // Arrange.
            var container = new TransientContainer();
            container.Register<IMock, Mock>();

            // Act.
            var result = container.Resolve<IMock>();

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<Mock>(result);
        }

        [Fact]
        public void Resolve_UnregisteredType_ThrowsResolutionFailedException()
        {
            // Arrange.
            var container = new TransientContainer();

            // Act, Assert.
            Assert.Throws<ResolutionFailedException>(() => {
                container.Resolve<IMock>();
            });
        }

        #endregion

        #region Mocks

        interface IMock
        {
        }

        class Mock : IMock
        {
        }

        #endregion
    }
}
