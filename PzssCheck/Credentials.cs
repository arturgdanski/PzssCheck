using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PzssCheck
{
    [Serializable]
    class Credentials
    {
        public Credentials()
        {
            m_entropy = Encoding.UTF8.GetBytes("PzssCheck");

            // TODO: generate this random byte and store in different place

            //using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            //{
            //    rng.GetBytes(m_entropy);
            //}

        }

        public Credentials(string username, string password)
            :this()
        {
            m_secureUsername = ProtectedData.Protect(Encoding.UTF8.GetBytes(username), m_entropy, DataProtectionScope.CurrentUser);
            m_securePassword = ProtectedData.Protect(Encoding.UTF8.GetBytes(password), m_entropy, DataProtectionScope.CurrentUser);
        }

        public string Login
        {
            get
            {
                return Encoding.UTF8.GetString(
                    ProtectedData.Unprotect(m_secureUsername, m_entropy, DataProtectionScope.CurrentUser));
            }
        }
        public string Password
        {
            get
            {
                return Encoding.UTF8.GetString(
                    ProtectedData.Unprotect(m_securePassword, m_entropy, DataProtectionScope.CurrentUser));
            }
        }

        private byte[] m_secureUsername;
        private byte[] m_securePassword;
        private byte[] m_entropy;

        public bool LoadCredentials(string filePath)
        {
            if (File.Exists("u.bin"))
                m_secureUsername = File.ReadAllBytes("u.bin");
            else
                return false;

            if (File.Exists("p.bin"))
                m_securePassword = File.ReadAllBytes("p.bin");
            else
                return false;
            
            // load entropy too

            return true;
        }

        public bool SaveCredentials(string filePath)
        {
            File.WriteAllBytes("u.bin", m_secureUsername);
            File.WriteAllBytes("p.bin", m_securePassword);

            // save entropy too

            return false;
        }

    }
}
