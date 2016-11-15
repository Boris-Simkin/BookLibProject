using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public interface ILoginView
    {
        //void SetMainView(List<AbstractItem> BooksSource, List<AbstractItem> JournalsSource);
        void SetMainView();
        void SetRegistrationView();
        event EventHandler<SubmitEventArgs> Submit;
        event EventHandler registerBtnClick;
        string StringFromServer { set; }
    }
}
