using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter2
{
    public interface IMainView
    {
        //List<AbstractItem> BooksSource { set; }
        //List<AbstractItem> JournalsSource { set; }
        void SetUserName(string userName);
    }
}
