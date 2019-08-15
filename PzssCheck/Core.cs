using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace PzssCheck
{
    class Core
    {
        private CredentialsManager m_credentialsManager;
        private LicenseProvider m_dataProvider;
        private LicenseParser m_licenseParser;
        
        public Core()
            : this(new CredentialsManager(), new LicenseProvider(), new LicenseParser())
        {
        }

        private Core(CredentialsManager credentialsLoader, LicenseProvider dataProvider, LicenseParser licenseParser)
        {
            m_credentialsManager = credentialsLoader;
            m_dataProvider = dataProvider;
            m_licenseParser = licenseParser;
        }

        public void Run()
        {
            if (m_credentialsManager.Load())
            {
                var licensesHtml = m_dataProvider.GetAllLicenses(m_credentialsManager.GetCredentials());
                if (licensesHtml != "")
                {
                    var licenses = m_licenseParser.Parse(licensesHtml);

                    if (licenses.Count() > 0)
                        foreach (var license in licenses)
                        {
                            Console.WriteLine(
                                "{0},{1},{2},{3},{4}",
                                license.LicenseName, license.LicenseNumber, license.LicenseType,
                                license.LicenseBeginDate, license.LicenseEndDate);
                        }
                    else
                        Console.WriteLine("Info: {0}", Properties.Resources.InfoNoLicensesFound);
                }
            }
            else
            {
                Console.WriteLine("{0}: {1}", Properties.Resources.ErrorTranslation, Properties.Resources.ErrorLoadCred);
            }
        }

    }
}
