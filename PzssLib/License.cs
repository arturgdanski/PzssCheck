using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PzssLib
{
    /// <summary>
    /// TODO:
    /// 1. Convert dates (including "bezterminowo" to date type
    /// 2. Parse License type ("PKS")
    /// </summary>
    [Serializable]
    public class License
    {
        public License()
        {
        }

        public License(string licenseName, string licenseNumber, string licenseType,
            string licenseBeginDate, string licenseEndDate)
        {
            LicenseName = licenseName;
            LicenseNumber = licenseNumber;
            LicenseType = licenseType;
            LicenseBeginDate = licenseBeginDate;
            LicenseEndDate = licenseEndDate;
        }

        public string LicenseName { get; private set; }
        
        public string LicenseNumber { get; private set; }

        public string LicenseType { get; private set; }

        public string LicenseBeginDate { get; private set; }

        public string LicenseEndDate { get; private set; }

    }
}
