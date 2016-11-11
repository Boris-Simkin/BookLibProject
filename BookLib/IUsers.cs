using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IUsers
    {
        Task<AuthenticationResult> Authentication(string username, string password);
        Task<AuthenticationResult> Registration(string username, string password, string firstName, string lastName);
    }
}
