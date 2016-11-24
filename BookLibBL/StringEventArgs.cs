using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibBL
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string str)
        {
            String = str;
        }

        public string String { get; private set; }
    }
}
