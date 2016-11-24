using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Flags]
    public enum EnumListType
    {
        Books = 1,
        Magazines = 2,
        MyBooks = 4,
        MyMagazines = 8
    }
}
