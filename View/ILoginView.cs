using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public interface ILoginView
    {
        event EventHandler<SubmitEventArgs> Submit;
        event EventHandler registerBtnClick;
        string StringFromServer { set; }
    }
}
