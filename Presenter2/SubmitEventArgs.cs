using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class SubmitEventArgs : EventArgs
    {
        public SubmitEventArgs(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public SubmitEventArgs(string username, string password, string firstname, string lastname)
        {
            Username = username;
            Password = password;
            Firstname = firstname;
            Lastname = lastname;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        //public bool IsAdmin { get; private set; }
    }
}
