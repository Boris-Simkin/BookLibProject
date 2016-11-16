using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Presenter
{
    static public class MainPresenter
    {
        private static readonly IItemsCollection _itemsCollection;
        private static Users _user = new Users();

        private static IItemDetailsPage _itemDetailsPage;
        private static IItemListPage _itemListPage;
        private static IItemDetailsPageAdmin _itemDetailsPageAdmin;
        private static IAddNewItemPage _addNewItemPage;
        private static IAdvancedSearchPage _advancedSearchPage;
        private static IMessagePage _messagePage;

        public static IAdvancedSearchPage AdvancedSearchPage
        {
            get
            {
                return _advancedSearchPage;
            }
            set
            {
                _advancedSearchPage = value;
                _advancedSearchPage.Submit += _advancedSearchPage_Submit;
            }
        }

        private static void _advancedSearchPage_Submit(object sender, ItemEventArgs e)
        {
            List<AbstractItem> result = _itemsCollection.AdvancedSearch(e.Item);

            _itemListPage.SourceList = result;
            //if (_itemListPage.IsBookList)
            //    _itemListPage.SourceList = result;
            //else
            //    _itemListPage.SourceList = result;
        }

        public static IMessagePage MessagePage
        {
            get
            {
                return _messagePage;
            }
            set
            {
                _messagePage = value;
                _messagePage.ButtonPressed += _messagePage_ButtonPressed;
            }
        }

        private static void _messagePage_ButtonPressed(object sender, EventArgs e)
        {
            _mainView.SetBooksListPage();
        }

        public static IItemDetailsPageAdmin ItemDetailsPageAdmin
        {
            get
            {
                return _itemDetailsPageAdmin;
            }
            set
            {
                _itemDetailsPageAdmin = value;
                _itemDetailsPageAdmin.Save += _itemDetailsPageAdmin_Submit;
                _itemDetailsPageAdmin.Delete += _itemDetailsPageAdmin_Delete;
            }
        }

        private static void _itemDetailsPageAdmin_Delete(object sender, ItemEventArgs e)
        {
            _itemsCollection.DeleteItem(e.Item);
        }

        private static void _itemDetailsPageAdmin_Submit(object sender, ItemEventArgs e)
        {
            _itemsCollection.UpdateItem(e.Item);
        }

        public static IItemListPage ItemListPage
        {
            get
            {
                return _itemListPage;
            }
            set
            {
                _itemListPage = value;
                _itemListPage.ItemClicked += _itemListPage_ItemClicked;
            }
        }

        private static void _itemListPage_ItemClicked(object sender, ItemEventArgs e)
        {
            _mainView.HideToolBar();
            _mainView.ClearTitle();
            if (_user.CurrentUser.IsAdmin)
            {
                _itemListPage.SetItemDetailsPage(true);
                _itemDetailsPageAdmin.SetContent(e.Item);
            }
            else
            {
                _itemListPage.SetItemDetailsPage(false);
                _itemDetailsPage.SetContent(e.Item);
            }
        }



        public static IAddNewItemPage AddNewItemPage
        {
            get
            {
                return _addNewItemPage;
            }
            set
            {
                _addNewItemPage = value;
                AddNewItemPage.Submit += AddNewItemPage_Submit;
            }
        }

        private async static void AddNewItemPage_Submit(object sender, ItemEventArgs e)
        {
            AuthenticationResult result = await _itemsCollection.AddItemToServer(e.Item);

            if (result == AuthenticationResult.Ok)
            {
                _itemsCollection.AddItem(e.Item);
                _mainView.ShowMessage("Item created");
            }
            else
                _mainView.ShowMessage("Connection failed");

        }

        public static IItemDetailsPage ItemDetailsPage
        {
            get
            {
                return _itemDetailsPage;
            }
            set
            {
                _itemDetailsPage = value;
            }
        }

        private static IRegisterView _registerView;
        public static IRegisterView RegisterView
        {
            get
            {
                return _registerView;
            }
            set
            {
                _registerView = value;
                _registerView.GoBack += _registerView_GoBack;
                _registerView.Submit += _registerView_Submit;
            }
        }

        private async static void _registerView_Submit(object sender, SubmitEventArgs e)
        {
            Task<AuthenticationResult> task = _user.Registration(e.Username, e.Password, e.Firstname, e.Lastname);
            switch (await task)
            {
                case AuthenticationResult.Ok:
                    _registerView.SetLoginCreatedPage();
                    break;
                case AuthenticationResult.ParamsIncorrect:
                    _registerView.StringFromServer = "This username already in use";
                    break;
                case AuthenticationResult.ConnectionFailed:
                    _registerView.StringFromServer = "Error connecting to the server";
                    break;
                default:
                    break;
            }
        }

        private static void _registerView_GoBack(object sender, EventArgs e)
        {
            _registerView.SetPreviusView();
        }

        private static ILoginView _loginView;
        public static ILoginView LoginView
        {
            get
            {
                return _loginView;
            }
            set
            {
                _loginView = value;
                _loginView.Submit += _loginView_Submit;
                _loginView.registerBtnClick += _loginView_registerBtnClick;
            }
        }

        private static void _loginView_registerBtnClick(object sender, EventArgs e)
        {
            _loginView.SetRegistrationView();
        }

        private static IMainView _mainView;
        public static IMainView MainView
        {
            get
            {
                return _mainView;
            }
            set
            {
                _mainView = value;
                _mainView.IsAdmin(_user.CurrentUser.IsAdmin);
                _mainView.BooksClicked += _mainView_BooksClicked;
                _mainView.MagazinesClicked += _mainView_MagazinesClicked;
                _mainView.MyBooksClicked += _mainView_MyBooksClicked;
                _mainView.MyMagazinesClicked += _mainView_MyMagazinesClicked;
                _mainView.SearchTextChanged += _mainView_SearchTextChanged;
                _mainView.Logout += _mainView_Logout;
            }
        }

        private static void _mainView_Logout(object sender, EventArgs e)
        {
            _loginView.Submit -= _loginView_Submit;
            _itemsCollection.ClearList();
        }

        private static void _mainView_SearchTextChanged(object sender, StringEventArgs e)
        {
            var newList = _itemsCollection.SearchByName(_itemListPage.IsBookList, e.String);
            if (!_itemListPage.SourceList.SequenceEqual(newList))
            {
                _mainView.SetCounter = newList.Count;
                _itemListPage.SourceList = newList;
            }
            if (e.String == string.Empty)
            {
                if (_itemListPage.IsBookList)
                    _itemListPage.SourceList = _itemsCollection.GetBooks();
                else
                    _itemListPage.SourceList = _itemsCollection.GetJournals();
            }
        }

        private static void _mainView_MyMagazinesClicked(object sender, EventArgs e)
        {
            _itemListPage.IsBookList = false;
        }

        private static void _mainView_MyBooksClicked(object sender, EventArgs e)
        {
            _itemListPage.IsBookList = true;
        }

        private static void _mainView_MagazinesClicked(object sender, EventArgs e)
        {
            var items = _itemsCollection.GetJournals();
            _itemListPage.IsBookList = false;
            _mainView.SetCounter = items.Count;
            _itemListPage.SourceList = items;
        }

        private static void _mainView_BooksClicked(object sender, EventArgs e)
        {
            var items = _itemsCollection.GetBooks();
            _itemListPage.IsBookList = true;
            _mainView.SetCounter = items.Count;
            _itemListPage.SourceList = items;
        }


        private async static void _loginView_Submit(object sender, SubmitEventArgs e)
        {
            Task<AuthenticationResult> task = _user.Authentication(e.Username, e.Password);

            switch (await task)
            {
                case AuthenticationResult.Ok:
                    await _itemsCollection.LoadDataFromServer();
                    LoginView.SetMainView();
                    _mainView.SetUserName(e.Username);
                    break;
                case AuthenticationResult.ParamsIncorrect:
                    LoginView.StringFromServer = "Incorrect username or password";
                    break;
                case AuthenticationResult.ConnectionFailed:
                    LoginView.StringFromServer = "Error connecting to the server";
                    break;
                default:
                    break;
            }
        }

        private async static void requestSubmit(string username, string password)
        {

        }

        static MainPresenter()
        {
            _itemsCollection = new ItemsCollection();
        }
    }
}
