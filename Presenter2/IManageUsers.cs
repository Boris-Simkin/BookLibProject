using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IManageUsers
    {
        event EventHandler<SubmitEventArgs> MakeAdmin;
        event EventHandler<SubmitEventArgs> DeleteUser;
        List<User> SourceList { get; set; }
    }
}
