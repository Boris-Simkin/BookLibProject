using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibBL
{
    public interface IAdvancedSearchPage
    {
        event EventHandler<ItemEventArgs> Submit;
    }
}
