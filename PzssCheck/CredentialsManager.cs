using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace PzssCheck
{
    class CredentialsManager
    {
        private Credentials m_credentials;

        public CredentialsManager()
        {
        }

        public bool Load()
        {
            m_credentials = new Credentials();
            if(m_credentials.LoadCredentials("credentials.bin"))
            {
                return true;
            }
            else
            {
                Console.WriteLine(Properties.Resources.InfoLoadCredFile);

                m_credentials = CreateNewCredentials();
                if(m_credentials != null)
                {
                    m_credentials.SaveCredentials("credentials.bin");
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
