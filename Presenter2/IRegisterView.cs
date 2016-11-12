using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter2
{
    public interface IRegisterView
    {
        event EventHandler<SubmitEventArgs> Submit;
        event EventHandler GoBack;
        //event EventHandler LoginCreated;
        string StringFromServer { set; }
        void SetLoginCreatedPage();
        void SetPreviusView();
        //void SetErrorMessage(string message);

    }
}
