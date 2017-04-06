using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Towers.DependencyInjection.Tests
{
    public class InversionOfControlContainerTests
    {
        #region IsRegistered Tests

        [Fact]
        public void IsRegistered_TypeRegistered_ReturnsTrue()
        {
            // Arrange.
            var type = typeof(IMock);
            var mock = new MockInversionOfControlContainer();
            mock.Register<IMock, Mock>();

            // Act.
            var result = mock.IsRegistered(type);

            // Assert.
            Assert.True(result, "expected a registered type to return true");
        }

        [Fact]
        public void IsRegistered_TypeNotRegistered_ReturnsFalse()
        {
            // Arrange.
            var type = typeof(IMock);
            var mock = new MockInversionOfControlContainer();

            // Act.
            var result = mock.IsRegistered(type);

            // Assert.
            Assert.False(result, "expected an unregistered type to return false");
        }

        #endregion

        #region ConstructType Tests

        [Fact]
        public void ConstructType_RegisteredTypeNull_ThrowsResolutionFailedException()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();

            // Act, Assert.
            var exception = Assert.Throws<ArgumentNullException>(() => 
            {
                mock.ConstructTypeMock(null);
            });
        }

        [Fact]
        public void ConstructType_NoConstructorParams_ReturnsInstance()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            mock.Register<IMock, Mock>();

            // Act.
            var result = mock.ConstructTypeMock(typeof(IMock));

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<Mock>(result);
        }

        [Fact]
        public void ConstructType_ConstructorWithUnusableUnRegisteredParams_ThrowsResolutionFailedException()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            mock.Register<IMock, MockWithInterfaceParams>();

            // Act, Assert.
            Assert.Throws<ResolutionFailedException>(() => 
            {
                mock.ConstructTypeMock(typeof(IMock));
            });
        }

        [Fact]
        public void ConstructType_ConstructorWithRegisteredParams_ReturnsInstance()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            mock.Register<IOther, Other>();
            mock.Register<IMock, MockWithInterfaceParams>();

            // Act.
            var result = mock.ConstructTypeMock(typeof(IMock));

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<MockWithInterfaceParams>(result);
        }

        [Fact]
        public void ConstructType_ConstructorWithUseableUnRegisteredParams_ReturnsInstance()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            mock.Register<IMock, MockWithParams>();

            // Act.
            var result = mock.ConstructTypeMock(typeof(IMock));

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<MockWithParams>(result);
        }

        #endregion

        #region ConstructParameters Tests

        [Fact]
        public void ConstructParameters_EmptyArray_ReturnsEmptyList()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();

            // Act.
            var result = mock.ConstructParametersMock(typeof(Mock), new ParameterInfo[0]);

            // Assert.
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ConstructParameters_UnregisteredParam_ThrowsResolutionFailedException()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            var type = typeof(MockWithInterfaceParams);
            var parameters = type.GetConstructors()[0].GetParameters();
            var expected = new ResolutionFailedException(parameters[0].ParameterType);

            // Act, Assert.
            var exception = Assert.Throws(typeof(ResolutionFailedException), () => 
            { 
                mock.ConstructParametersMock(type, parameters);
            });
            Assert.Equal(expected.Message, exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void ConstructParameters_ParamWithoutConstructor_ThrowsUnsupportedTypeException()
        {
            // Arrange.
            // This is used to provide a overriden result for TryGetRegisteredTypeImplementation();
            var parameterType = typeof(IOther);
            var mockTryGetRegisteredTypeImplementationResult = new TryGetRegisteredTypeImplementationResult
            {
                Result = true,
                Implementation = parameterType
            };

            var mock = new MockInversionOfControlContainer(mockTryGetRegisteredTypeImplementationResult);
            var type = typeof(MockWithInterfaceParams);
            var parameters = type.GetConstructors()[0].GetParameters();
            var temp = new UnsupportedTypeException(InversionOfControlContainer.NO_CONSTRUCTOR_MESSAGE, parameterType);
            var expected = new UnsupportedTypeException(temp.Message, parameterType);

            // Act, Assert.
            var exception = Assert.Throws(typeof(UnsupportedTypeException), () =>
            {
                mock.ConstructParametersMock(type, parameters);
            });
            Assert.Equal(expected.Message, exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        #endregion

        #region GetImplementationConstructorWithParameters Tests

        [Fact]
        public void GetImplementationConstructorWithParameters_TypeWithConstructorNoParameters_ReturnsValidInformation()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();

            // Act.
            var result = mock.GetImplementationConstructorWithParametersMock(typeof(Mock));

            // Assert.
            Assert.NotNull(result);
            Assert.NotNull(result.Constructor);
            Assert.NotNull(result.Parameters);
            Assert.Empty(result.Parameters);
        }

        [Fact]
        public void GetImplementationConstructorWithParameters_TypeWithConstructorAndParameters_ReturnsValidInformation()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();

            // Act.
            var result = mock.GetImplementationConstructorWithParametersMock(typeof(MockWithParams));

            // Assert.
            Assert.NotNull(result);
            Assert.NotNull(result.Constructor);
            Assert.NotNull(result.Parameters);
            Assert.NotEmpty(result.Parameters);
        }

        [Fact]
        public void GetImplementationConstructorWithParameters_TypeWithoutConstructor_ThrowsUnsupportedTypeException()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            var type = typeof(IMock);
            var expected = new UnsupportedTypeException(InversionOfControlContainer.NO_CONSTRUCTOR_MESSAGE, type);

            // Act, Assert.
            var exception = Assert.Throws<UnsupportedTypeException>(() => 
            {
                mock.GetImplementationConstructorWithParametersMock(type);
            });
            Assert.Equal(expected.Message, exception.Message);
        }

        #endregion

        #region HasEmptyContructor Tests

        [Fact]
        public void HasEmptyContructor_TypeWithoutContructor_ReturnsFalse()
        {
            // Arrange, Act.
            var result = new MockInversionOfControlContainer().HasEmptyContructorMock(typeof(IMock));

            // Assert.
            Assert.False(result, "expected type without a constructor to return false");
        }

        [Fact]
        public void HasEmptyContructor_TypeWithContructorParams_ReturnsFalse()
        {
            // Arrange, Act.
            var result = new MockInversionOfControlContainer().HasEmptyContructorMock(typeof(MockWithParams));

            // Assert.
            Assert.False(result, "expected type without a parameterless constructor to return false");
        }

        [Fact]
        public void HasEmptyContructor_TypeWithContructorWithNoParams_ReturnsTrue()
        {
            // Arrange, Act.
            var result = new MockInversionOfControlContainer().HasEmptyContructorMock(typeof(Mock));

            // Assert.
            Assert.True(result, "expected type with a parameterless constructor to return true");
        }

        #endregion

        #region TryGetRegisteredTypeImplementation Tests

        [Fact]
        public void TryGetRegisteredTypeImplementation_RegisteredTypeNull_ReturnsFalseAndOutImplementationNull()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            Type implementation;

            // Act.
            var result = mock.TryGetRegisteredTypeImplementationMock(null, out implementation);

            // Assert.
            Assert.False(result, "expected an unknown type to return false");
            Assert.Null(implementation);
        }

        [Fact]
        public void TryGetRegisteredTypeImplementation_ContainedInRegisteredTypes_ReturnsTrueAndOutImplementation()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            mock.Register<IMock, Mock>();
            Type implementation;

            // Act.
            var result = mock.TryGetRegisteredTypeImplementationMock(typeof(IMock), out implementation);

            // Assert.
            Assert.True(result, "expected a registered type to return true");
            Assert.NotNull(implementation);
        }

        [Fact]
        public void TryGetRegisteredTypeImplementation_NotContainedInRegisteredTypes_ReturnsFalseAndOutImplementationNull()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            Type implementation;

            // Act.
            var result = mock.TryGetRegisteredTypeImplementationMock(typeof(IMock), out implementation);

            // Assert.
            Assert.False(result, "expected an unknown type to return false");
            Assert.Null(implementation);
        }

        [Fact]
        public void TryGetRegisteredTypeImplementation_NotContainedInRegisteredTypesButHasParameterlessConstructor_ReturnsTrueAndOutImplementation()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            Type implementation;

            // Act.
            var result = mock.TryGetRegisteredTypeImplementationMock(typeof(Mock), out implementation);

            // Assert.
            Assert.True(result, "expected an unknown type with a parameterless constructor to return true");
            Assert.NotNull(implementation);
        }

        [Fact]
        public void TryGetRegisteredTypeImplementation_NotContainedInRegisteredTypesAndNoParameterlessConstructor_ReturnsFalseAndOutImplementationNull()
        {
            // Arrange.
            var mock = new MockInversionOfControlContainer();
            Type implementation;

            // Act.
            var result = mock.TryGetRegisteredTypeImplementationMock(typeof(MockWithParams), out implementation);

            // Assert.
            Assert.False(result, "expected an unknown type with no parameterless constructor to return false");
            Assert.Null(implementation);
        }

        #endregion

        #region Mocks

        interface IMock
        {
        }

        class Mock: IMock
        {
        }

        class MockWithParams : IMock
        {
            public MockWithParams(Mock param)
            {
            }
        }

        interface IOther
        {
        }

        class Other: IOther
        {
        }

        class MockWithInterfaceParams : IMock
        {
            public MockWithInterfaceParams(IOther IMock)
            {
            }
        }

        class MockInversionOfControlContainer : InversionOfControlContainer
        {
            private bool? _hasEmptyContructorMock;
            private TryGetRegisteredTypeImplementationResult _tryGetRegisteredTypeImplementationResult;

            public MockInversionOfControlContainer()
            {
            }

            public MockInversionOfControlContainer(bool hasEmptyContructorMockResult)
            {
                _hasEmptyContructorMock = hasEmptyContructorMockResult;
            }

            public MockInversionOfControlContainer(TryGetRegisteredTypeImplementationResult tryGetRegisteredTypeImplementationResult)
            {
                _tryGetRegisteredTypeImplementationResult = tryGetRegisteredTypeImplementationResult;
            }

            public override void Register<T, TImplementation>()
            {
                _registeredTypes[typeof(T)] = typeof(TImplementation);
            }

            public override T Resolve<T>()
            {
                var registeredImplementation = _registeredTypes[typeof(T)];
                return ConstructType(registeredImplementation) as T;
            }

            public object ConstructTypeMock(Type registeredType)
            {
                return ConstructType(registeredType);
            }

            public List<object> ConstructParametersMock(Type registeredType, ParameterInfo[] parameters)
            {
                return ConstructParameters(registeredType, parameters);
            }

            public ImplementationInfo GetImplementationConstructorWithParametersMock(Type implementation)
            {
                return GetImplementationConstructorWithParameters(implementation);
            }

            public bool HasEmptyContructorMock(Type type)
            {
                return HasEmptyContructor(type);
            }

            public bool TryGetRegisteredTypeImplementationMock(Type registeredType, out Type implementation)
            {
                return TryGetRegisteredTypeImplementation(registeredType, out implementation);
            }

            protected override bool HasEmptyContructor(Type type)
            {
                if (_hasEmptyContructorMock.HasValue)
                    return _hasEmptyContructorMock.Value;

                return base.HasEmptyContructor(type);
            }

            protected override bool TryGetRegisteredTypeImplementation(Type registeredType, out Type implementation)
            {
                if (_tryGetRegisteredTypeImplementationResult != null)
                {
                    implementation = _tryGetRegisteredTypeImplementationResult.Implementation;
                    return _tryGetRegisteredTypeImplementationResult.Result;
                }

                return base.TryGetRegisteredTypeImplementation(registeredType, out implementation);
            }
        }

        class TryGetRegisteredTypeImplementationResult
        {
            public bool Result { get; set; }
            public Type Implementation { get; set; }
        }

        #endregion
    }
}
