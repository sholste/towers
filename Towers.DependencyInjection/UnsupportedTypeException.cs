using System;

namespace Towers.DependencyInjection
{
    public sealed class UnsupportedTypeException: Exception
    {
        private const string TYPE_MESSAGE = "type=";

        public UnsupportedTypeException(string message, Type T)
            : base(string.Format("{0}, {1}{2}", message, TYPE_MESSAGE, T.FullName))
        {
            ReasonMessage = message;
        }

        public UnsupportedTypeException(string message, Type T, string additionalInformation)
            : base(string.Format("{0}, {1}{2}", message, TYPE_MESSAGE, T.FullName),
                  new Exception(additionalInformation))
        {
            ReasonMessage = message;
        }

        public string ReasonMessage { get; private set; }
    }
}
