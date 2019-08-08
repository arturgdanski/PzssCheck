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
            m_entropy = new byte[1024];
        }

        public Credentials(string username, string password)
            :this()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(m_entropy);
            }

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

        public bool LoadCredentials(string fileName)
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PzssCheck");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var stream = new FileStream(Path.Combine(directory, fileName), FileMode.OpenOrCreate))
            {
                using (var reader = new BinaryReader(stream))
                {
                    try
                    {
                        var usernameSize = reader.ReadInt32();
                        var passwordSize = reader.ReadInt32();
                        m_secureUsername = reader.ReadBytes(usernameSize);
                        m_securePassword = reader.ReadBytes(passwordSize);
                    }
                    catch (EndOfStreamException)
                    {
                        return false;
                    }
                }
            }
            
            // Load entropy
            var entropyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PzssCheck");
            if (!Directory.Exists(entropyPath))
                return false;

            using (var stream = new FileStream(Path.Combine(entropyPath, "entropy.bin"), FileMode.OpenOrCreate))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var len = reader.ReadInt32();
                    m_entropy = reader.ReadBytes(len);
                }
            }

            return true;
        }

        public bool SaveCredentials(string fileName)
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PzssCheck");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var stream = new FileStream(Path.Combine(directory, fileName), FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(m_secureUsername.Length);
                    writer.Write(m_securePassword.Length);
                    writer.Write(m_secureUsername);
                    writer.Write(m_securePassword);
                }
            }

            // save entropy too
            var entropyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PzssCheck");
            if (!Directory.Exists(entropyPath))
                Directory.CreateDirectory(entropyPath);

            using (var stream = new FileStream(Path.Combine(entropyPath, "entropy.bin"), FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(m_entropy.Length);
                    writer.Write(m_entropy);
                }
            }

            return false;
        }

    }
}
