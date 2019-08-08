using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace PzssCheck
{
    /// <summary>
    /// TODO:
    /// 1. Remove whole ini-parser nuget package and references
    /// 2. Save/Load Credentials using secure Save/Load methods from Credentials class
    /// 3. If no credentials file in system (store in AppData?), then ask to create
    /// 4. Create secure input for entering password in console
    /// </summary>
    class CredentialsManager
    {
        private Credentials m_credentials;

        public CredentialsManager()
        {

        }

        public bool Load()
        {
            m_credentials = new Credentials();
            if(m_credentials.LoadCredentials(" "))
            {
                return true;
            }
            else
            {
                Console.WriteLine(Properties.Resources.InfoLoadCredFile);

                m_credentials = CreateNewCredentials();
                if(m_credentials != null)
                {
                    m_credentials.SaveCredentials(" ");
                    return true;
                }
            }

            return false;
        }

        public Credentials GetCredentials()
        {
            return m_credentials;
        }
        
        private Credentials CreateNewCredentials()
        {
            string username;
            string password;

            Console.WriteLine(Properties.Resources.InfoNewCredentialsWelcomeString);
            var ret = Console.ReadLine();
            if(ret.ToLower() != Properties.Resources.UserResponseNegative)
            {
                Console.WriteLine(Properties.Resources.InfoPleaseEnterLogin);
                username = Console.ReadLine();
                Console.WriteLine(Properties.Resources.InfoPleaseEnterPassword);
                password = Console.ReadLine();
                Console.Clear();
            }
            else
            {
                return null;
            }

            return new Credentials(username, password);
        }
        
    }
}
