using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibBL
{
    public interface IRegisterView
    {
        event EventHandler<UserEventArgs> Submit;
        event EventHandler GoBack;
        string StringFromServer { set; }
        void ShowMessage(string message);
        void SetPreviusView();
        void RequestFinished();
    }
}
