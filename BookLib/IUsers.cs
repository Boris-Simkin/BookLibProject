using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IUsers
    {
        // Users.ResultEnum Authentication(string username, string password);
         Task<Users.AuthenticationResult> Authentication(string username, string password);
    }
}
