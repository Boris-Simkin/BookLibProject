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
        private static Users _users = new Users();

        private static IItemDetailsPage _itemDetailsPage;
        private static IItemListPage _itemListPage;
        private static IItemDetailsPageAdmin _itemDetailsPageAdmin;
        private static IAddNewItemPage _addNewItemPage;
        private static IAdvancedSearchPage _advancedSearchPage;
        private static IManageUsers _manageUsers;

        public static IManageUsers ManageUsersPage
        {
            get
            {
                return _manageUsers;
            }
            set
            {
                _manageUsers = value;
                _manageUsers.DeleteUser += _manageUsers_DeleteUser;
                _manageUsers.MakeAdmin += _manageUsers_MakeAdmin;
                _manageUsers.PageLoaded += _manageUsers_PageLoaded;
            }
        }

        private static void _manageUsers_PageLoaded(object sender, EventArgs e)
        {
            //Loading users except the administrators
            var regularUsers = _users.GetUsers().Where(user => user.IsAdmin == false);
            ManageUsersPage.SourceList = new List<User>(regularUsers);
        }

        private async static void _manageUsers_MakeAdmin(object sender, UserEventArgs e)
        {
            Task<ResultFromServer> task = _users.MakeUserAdmin(e.User);

            if (await task == ResultFromServer.Ok)
                _users.GetUsers().Remove(e.User);
            else
                _mainView.ShowMessage("Connection failed");

            ManageUsersPage.RequestFinished();
        }

        private async static void _manageUsers_DeleteUser(object sender, UserEventArgs e)
        {
            Task<ResultFromServer> task = _users.RemoveUserFromServer(e.User);

            if (await task == ResultFromServer.Ok)
                _users.GetUsers().Remove(e.User);
                
            else
                _mainView.ShowMessage("Connection failed");

            ManageUsersPage.RequestFinished();
        }

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
            _mainView.SetTitle("Result");
            _mainView.SetCounter = result.Count;
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
                _itemDetailsPageAdmin.Save += _itemDetailsPageAdmin_UpdateItem;
                _itemDetailsPageAdmin.Delete += _itemDetailsPageAdmin_Delete;
            }
        }

        private static async void _itemDetailsPageAdmin_Delete(object sender, ItemEventArgs e)
        {
            ResultFromServer result = await _itemsCollection.DeleteFromServer(e.Item);

            if (result == ResultFromServer.Ok)
            {
                var temp = e.Item;
                _itemsCollection.DeleteItem(e.Item);
                if (e.Item is Book)
                    _mainView.SetBooksListPage();
                else
                    _mainView.SetMagazinesListPage();
            }
            else
                _mainView.ShowMessage("Connection failed");
        }

        private static async void _itemDetailsPageAdmin_UpdateItem(object sender, ItemEventArgs e)
        {
            ResultFromServer result = await _itemsCollection.UpdateInServer(e.Item);

            if (result == ResultFromServer.Ok)
            {
                _itemsCollection.UpdateItem(e.Item);

                if (e.Item is Book)
                    _mainView.ShowMessage("Book updated.");
                else
                    _mainView.ShowMessage("Magazine updated.");

            }
            else
                _mainView.ShowMessage("Connection failed");
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
            if (_users.CurrentUser.IsAdmin)
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
            ResultFromServer result = await _itemsCollection.AddItemToServer(e.Item);

            if (result == ResultFromServer.Ok)
            {
                _itemsCollection.AddItem(e.Item);

                if (e.Item is Book)
                {
                    _mainView.ShowMessage("Book created.");
                    _mainView.SetBooksListPage();
                }
                else
                {
                    _mainView.ShowMessage("Magazine created.");
                    _mainView.SetMagazinesListPage();
                }
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

        private async static void _registerView_Submit(object sender, UserEventArgs e)
        {
            Task<ResultFromServer> task = _users.Registration(e.User);
            switch (await task)
            {
                case ResultFromServer.Ok:
                    _registerView.ShowMessage("Your account was successfully created!");
                    _registerView.SetPreviusView();
                    break;
                case ResultFromServer.ParamsIncorrect:
                    _registerView.StringFromServer = "This username already in use";
                    break;
                case ResultFromServer.ConnectionFailed:
                    _registerView.StringFromServer = "Error connecting to the server";
                    break;
                default:
                    break;
            }
            _registerView.RequestFinished();
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
                _mainView.IsAdmin(_users.CurrentUser.IsAdmin);
                _mainView.BooksClicked += _mainView_BooksClicked;
                _mainView.MagazinesClicked += _mainView_MagazinesClicked;
                _mainView.MyBooksClicked += _mainView_MyBooksClicked;
                _mainView.MyMagazinesClicked += _mainView_MyMagazinesClicked;
                _mainView.SearchTextChanged += _mainView_SearchTextChanged;
                _mainView.ManageUsersClicked += _mainView_ManageUsersClicked;
                _mainView.Logout += _mainView_Logout;
            }
        }

        private static void _mainView_ManageUsersClicked(object sender, EventArgs e)
        {
            //await Task.Delay(TimeSpan.FromSeconds(30));
            //ManageUsersPage.SourceList = _user.GetUsers();
        }


        private static void _mainView_Logout(object sender, EventArgs e)
        {
            _loginView.Submit -= _loginView_Submit;
            _itemsCollection.ClearList();
            _users.ClearList();
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


        private async static void _loginView_Submit(object sender, UserEventArgs e)
        {
            Task<ResultFromServer> task = _users.Authentication(e.User);

            switch (await task)
            {
                case ResultFromServer.Ok:

                    var result = await _itemsCollection.LoadDataFromServer();
                    if (result == ResultFromServer.Ok)
                    {
                        if (!_users.CurrentUser.IsAdmin)
                        {
                            LoginView.SetMainView();
                            _mainView.SetUserName(e.User.Username);
                        }
                        else
                        {
                            Task<ResultFromServer> task2 = _users.GetUsersFromServer();
                            if (await task2 == ResultFromServer.Ok)
                            {
                                LoginView.SetMainView();
                                _mainView.SetUserName(_users.CurrentUser.Username);
                            }
                            else
                                LoginView.StringFromServer = "Error retriving users from server";
                        }
                    }
                    else
                        LoginView.StringFromServer = "Could not connect to server db";
                    break;
                case ResultFromServer.ParamsIncorrect:
                    LoginView.StringFromServer = "Incorrect username or password";
                    break;
                case ResultFromServer.ConnectionFailed:
                    LoginView.StringFromServer = "Error connecting to the server";
                    break;
                default:
                    break;
            }

            LoginView.RequestFinished();
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
