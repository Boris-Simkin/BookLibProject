using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class ItemEventArgs : EventArgs
    {
        public ItemEventArgs(AbstractItem item)
        {
            Item = item;
        }

        public AbstractItem Item { get; private set; }
    }
}
