using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PzssCheck
{
    [Serializable]
    class Credentials
    {
        public Credentials()
        {
        }

        public Credentials(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; private set; }
        public string Password { get; private set; }
    }
}
