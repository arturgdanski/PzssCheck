using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PzssCheck
{
    [Serializable]
    class License
    {
        public License()
        {
        }

        public License(string licenseName, string licenseNumber)
        {
            LicenseName = licenseName;
            LicenseNumber = licenseNumber;
        }

        public string LicenseName { get; private set; }
        
        public string LicenseNumber { get; private set; }
    }
}
