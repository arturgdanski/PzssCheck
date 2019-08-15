using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

using PzssLib;

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
            Console.WriteLine(Properties.Resources.PzssCheckWelcomeString);

            if (m_credentialsManager.Load())
            {
                PrintLicensesAsCsv(RetrieveLicenses());
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("{0}: {1}", Properties.Resources.InfoTranslation, Properties.Resources.InfoLoadCredFile);
                
                if(m_credentialsManager.CreateNewCredentials())
                {
                    if (!m_credentialsManager.Save())
                        Console.WriteLine("{0}: {1}", Properties.Resources.ErrorTranslation, Properties.Resources.ErrorSaveCred);
                    PrintLicensesAsCsv(RetrieveLicenses());
                    Console.ReadLine();
                }
            }
        }

        private License[] RetrieveLicenses()
        {
            License[] licenses = null;

            var licensesHtml = m_dataProvider.GetAllLicenses(m_credentialsManager.GetCredentials());
            if (licensesHtml != "")
                licenses = m_licenseParser.Parse(licensesHtml);

            return licenses;
        }

        private void PrintLicensesAsCsv(License[] licenses)
        {
            if (licenses != null)
            {
                if (licenses.Count() > 0)
                    foreach (var license in licenses)
                    {
                        Console.WriteLine(
                            "{0},{1},{2},{3},{4}",
                            license.LicenseName, license.LicenseNumber, license.LicenseType,
                            license.LicenseBeginDate, license.LicenseEndDate);
                    }
                else
                    Console.WriteLine("{0}: {1}", Properties.Resources.InfoTranslation, Properties.Resources.InfoNoLicensesFound);
            }
            else
                Console.WriteLine("{0}: {1}", Properties.Resources.ErrorTranslation, Properties.Resources.ErrorUnknownError);
        }

    }
}
