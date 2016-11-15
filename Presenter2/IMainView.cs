using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IMainView
    {
        void HideToolBar();
        void IsAdmin(bool value);
        int SetCounter { set; }
        void SetUserName(string userName);
        event EventHandler MagazinesClicked;
        event EventHandler BooksClicked;
        event EventHandler MyMagazinesClicked;
        event EventHandler MyBooksClicked;
    }
}
