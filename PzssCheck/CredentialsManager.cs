using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using IniParser;
using IniParser.Model;
using System.Configuration;

namespace PzssCheck
{
    class CredentialsManager
    {
        private string m_filePath;
        private Credentials m_credentials;

        public CredentialsManager()
        {
            var fileName = Properties.Resources.CredentialsFile;
            if(fileName != null)
            {
                m_filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }
        }

        public bool Load()
        {
            if (File.Exists(m_filePath))
            {
                LoadCredentials();
                return true;
            }
            else
            {
                Console.WriteLine("Error: {0}", Properties.Resources.ErrorLoadCredFile);
                return false;
            }
        }

        public Credentials GetCredentials()
        {
            return m_credentials;
        }

        private void LoadCredentials()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(m_filePath);
            
            var username = data["Credentials"]["Login"];
            var password = data["Credentials"]["Password"];

            if(username != null && password != null)
            {
                m_credentials = new Credentials(username, password);
            }
        }
        
    }
}
