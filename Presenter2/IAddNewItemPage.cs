using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IAddNewItemPage
    {
        event EventHandler<ItemEventArgs> Submit;
    }
}
