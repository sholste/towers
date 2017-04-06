using System;

namespace Towers.DependencyInjection
{
    public sealed class ResolutionFailedException: Exception
    {
        private const string MESSAGE = "Resolution of the dependency failed, type=";

        public ResolutionFailedException(Type T)
            : base(string.Format("{0}{1}", MESSAGE, T.FullName))
        {
        }

        public ResolutionFailedException(Type T, string additionalInformation)
            : base(string.Format("{0}{1}", MESSAGE, T.FullName),
                  new Exception(additionalInformation))
        {
        }
    }
}
