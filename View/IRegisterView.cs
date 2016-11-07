using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public interface IRegisterView
    {
        event EventHandler submit;
        event EventHandler goBack;
    }
}
