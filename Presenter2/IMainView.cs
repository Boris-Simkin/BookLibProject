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
        void SetTitle(string title);
        void SetUserName(string userName);
        void SetBooksListPage();
        void SetMagazinesListPage();
        void ClearTitle();
        void ShowMessage(string str);
        int SetCounter { set; }

        event EventHandler<StringEventArgs> SearchTextChanged;
        event EventHandler MainViewLoaded;
        event EventHandler MagazinesClicked;
        event EventHandler BooksClicked;
        event EventHandler MyMagazinesClicked;
        event EventHandler MyBooksClicked;
        event EventHandler Logout;
    }
}
