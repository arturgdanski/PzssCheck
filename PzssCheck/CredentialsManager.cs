using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

using PzssLib;

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
                return true;
            return false;
        }

        public bool Save()
        {
            if(m_credentials != null)
                return m_credentials.SaveCredentials("credentials.bin");
            return false;
        }

        public Credentials GetCredentials()
        {
            return m_credentials;
        }
        
        public bool CreateNewCredentials()
        {
            string username = "";
            string password = "";

            Console.Write(Properties.Resources.InfoNewCredentialsWelcomeString);
            var ret = Console.ReadLine();

            if(ret.ToLower() != Properties.Resources.UserResponseNegative)
            {
                Console.WriteLine(Properties.Resources.InfoPleaseEnterLogin);
                username = Console.ReadLine();
                Console.WriteLine(Properties.Resources.InfoPleaseEnterPassword);
                password = Console.ReadLine();
                Console.Clear();
                m_credentials = new Credentials(username, password);
            }
            else
            {
                m_credentials = null;
                return false;
            }

            return true;
        }
        
    }
}
