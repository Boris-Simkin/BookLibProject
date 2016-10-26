using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibProject
{
    public interface ILoginView
    {
        event EventHandler<SubmitEventArgs> Submit;
        void SetUserPage();
        void SetAdminPage();
    }
}
