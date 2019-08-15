using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PzssLib.Exceptions
{
    [Serializable]
    public class LicenseProviderException : Exception
    {
        public LicenseProviderException() { }

        public LicenseProviderException(string message)
            : base(message)
        {
        }

        public LicenseProviderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
