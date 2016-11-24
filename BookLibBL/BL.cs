using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace BookLibBL
{
    static public class BL
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

            switch (await task)
            {
                case ResultFromServer.Yes:
                    ManageUsersPage.RequestFinished(true);
                    break;
                case ResultFromServer.No:
                    _mainView.ShowMessage("This user does not exist");
                    ManageUsersPage.RequestFinished(true);
                    break;
                case ResultFromServer.ConnectionFailed:
                    _mainView.ShowMessage("Connection failed");
                    ManageUsersPage.RequestFinished(false);
                    break;
                default:
                    break;
            }
        }

        private async static void _manageUsers_DeleteUser(object sender, UserEventArgs e)
        {
            await _users.GetUserItemsGuid(e.User);
            Task<ResultFromServer> task = _users.RemoveUserFromServer(e.User);

            switch (await task)
            {
                case ResultFromServer.Yes:
                    _itemsCollection.ReturnAllUserItems(e.User);
                    _users.GetUsers().Remove(e.User);
                    ManageUsersPage.RequestFinished(true);
                    break;
                case ResultFromServer.No:
                    _mainView.ShowMessage("This user does not exist");
                    ManageUsersPage.RequestFinished(true);
                    break;
                case ResultFromServer.ConnectionFailed:
                    _mainView.ShowMessage("Connection failed");
                    ManageUsersPage.RequestFinished(false);
                    break;
                default:
                    break;
            }
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
                _itemDetailsPageAdmin.Borrow += _itemDetailsPageAdmin_Borrow;
            }
        }

        private static void _itemDetailsPageAdmin_Borrow(object sender, ItemEventArgs e)
        {
            BorrowReturnItem(e.Item);
        }

        private async static void BorrowReturnItem(AbstractItem item)
        {
            Task<ResultFromServer> task;
            task = _itemsCollection.BorrowReturnServer(_users.CurrentUser, item, !_users.CurrentUser.IsReading(item));

            switch (await task)
            {
                case ResultFromServer.Yes:

                    if (!_users.CurrentUser.IsReading(item))
                    {
                        _users.CurrentUser.MyItems.Add(item.Guid);
                        item.BorrowedCopies++;
                        if (_users.CurrentUser.IsAdmin)
                            _itemDetailsPageAdmin.BorrowReturnSucceeded(true);
                        else
                            _itemDetailsPage.BorrowReturnSucceeded(true);

                        if (item is Book)
                            _mainView.ShowMessage("You borrowed the book");
                        else
                            _mainView.ShowMessage("You borrowed the magazine");


                    }
                    else
                    {
                        _users.CurrentUser.MyItems.Remove(item.Guid);
                        item.BorrowedCopies--;
                        if (_users.CurrentUser.IsAdmin)
                            _itemDetailsPageAdmin.BorrowReturnSucceeded(false);
                        else
                            _itemDetailsPage.BorrowReturnSucceeded(false);


                        if (item is Book)
                            _mainView.ShowMessage("You returned a book");
                        else
                            _mainView.ShowMessage("You returned a magazine");

                    }

                    break;
                case ResultFromServer.No:
                    _mainView.ShowMessage("This item does not exist");
                    break;
                case ResultFromServer.ConnectionFailed:
                    _mainView.ShowMessage("Connection failed");
                    break;
                default:
                    break;
            }

        }

        private static async void _itemDetailsPageAdmin_Delete(object sender, ItemEventArgs e)
        {
            if (e.Item.BorrowedCopies > 0 )
            {
                _mainView.ShowMessage($"Before you can remove the {e.Item} all users must return all copies");
                return;
            }

            ResultFromServer task = await _itemsCollection.DeleteFromServer(e.Item);
            AbstractItem temp;

            if (task == ResultFromServer.No)
                _mainView.ShowMessage($"This {e.Item} does not exist");

            if (task == ResultFromServer.Yes || task == ResultFromServer.No)
            {
                temp = e.Item;
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
            switch (await _itemsCollection.UpdateItemInServer(e.Item))
            {
                case ResultFromServer.Yes:
                    _itemsCollection.UpdateItem(e.Item);

                    if (e.Item is Book)
                        _mainView.ShowMessage("Book updated");
                    else
                        _mainView.ShowMessage("Magazine updated");
                    _itemDetailsPageAdmin.SaveSucceeded();
                    break;
                case ResultFromServer.No:
                    _mainView.ShowMessage("This item does not exist");
                    break;
                case ResultFromServer.ConnectionFailed:
                    _mainView.ShowMessage("Connection failed");
                    break;
                default:
                    break;
            }
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
                _itemDetailsPageAdmin.SetContent(e.Item, _users.CurrentUser.IsReading(e.Item));
            }
            else
            {
                _itemListPage.SetItemDetailsPage(false);
                _itemDetailsPage.SetContent(e.Item, _users.CurrentUser.IsReading(e.Item));
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

            if (result == ResultFromServer.Yes)
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
                _itemDetailsPage.Borrow += _itemDetailsPage_Borrow;
            }
        }

        private static void _itemDetailsPage_Borrow(object sender, ItemEventArgs e)
        {
            BorrowReturnItem(e.Item);
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
                case ResultFromServer.Yes:
                    _registerView.ShowMessage("Your account was successfully created!");
                    _registerView.SetPreviusView();
                    break;
                case ResultFromServer.No:
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
                _mainView.MainViewLoaded += _mainView_MainViewLoaded;
                _mainView.IsAdmin(_users.CurrentUser.IsAdmin);
                _mainView.BooksClicked += _mainView_BooksClicked;
                _mainView.MagazinesClicked += _mainView_MagazinesClicked;
                _mainView.MyBooksClicked += _mainView_MyBooksClicked;
                _mainView.MyMagazinesClicked += _mainView_MyMagazinesClicked;
                _mainView.SearchTextChanged += _mainView_SearchTextChanged;
                _mainView.Logout += _mainView_Logout;
            }
        }

        private static void _mainView_MainViewLoaded(object sender, EventArgs e)
        {
            MainViewBookList();
        }

        private static void _mainView_Logout(object sender, EventArgs e)
        {
            _loginView.Submit -= _loginView_Submit;
            _itemsCollection.ClearList();
            _users.ClearList();
        }

        private static void _mainView_SearchTextChanged(object sender, StringEventArgs e)
        {
            var newList = _itemsCollection.SearchByName(_itemListPage.ListType, e.String, _users.CurrentUser);

            if (!_itemListPage.SourceList.SequenceEqual(newList))
            {
                _mainView.SetCounter = newList.Count;
                _itemListPage.SourceList = newList;
            }
            if (e.String == string.Empty)
            {
                switch (_itemListPage.ListType)
                {
                    case EnumListType.Books:
                        _itemListPage.SourceList = _itemsCollection.GetBooks();
                        break;
                    case EnumListType.Magazines:
                        _itemListPage.SourceList = _itemsCollection.GetJournals();
                        break;
                    case EnumListType.MyBooks:
                        _itemListPage.SourceList = _itemsCollection.GetUserBooks(_users.CurrentUser);
                        break;
                    case EnumListType.MyMagazines:
                        _itemListPage.SourceList = _itemsCollection.GetUserJournals(_users.CurrentUser);
                        break;
                }
            }
        }

        private static void _mainView_MyMagazinesClicked(object sender, EventArgs e)
        {
            var items = _itemsCollection.GetUserJournals(_users.CurrentUser);
            _itemListPage.ListType = EnumListType.MyMagazines;
            _mainView.SetCounter = items.Count;
            _itemListPage.SourceList = items;
        }

        private static void _mainView_MyBooksClicked(object sender, EventArgs e)
        {
            var items = _itemsCollection.GetUserBooks(_users.CurrentUser);
            _itemListPage.ListType = EnumListType.MyBooks;
            _mainView.SetCounter = items.Count;
            _itemListPage.SourceList = items;
        }

        private static void _mainView_MagazinesClicked(object sender, EventArgs e)
        {
            var items = _itemsCollection.GetJournals();
            _itemListPage.ListType = EnumListType.Magazines;
            _mainView.SetCounter = items.Count;
            _itemListPage.SourceList = items;
        }

        private static void _mainView_BooksClicked(object sender, EventArgs e)
        {
            MainViewBookList();
        }

        private static void MainViewBookList()
        {
            var items = _itemsCollection.GetBooks();
            _itemListPage.ListType = EnumListType.Books;
            _mainView.SetCounter = items.Count;
            _itemListPage.SourceList = items;
        }


        private async static void _loginView_Submit(object sender, UserEventArgs e)
        {
            _users.CurrentUser = new User();
            Task<ResultFromServer> task = _users.Authentication(e.User);

            switch (await task)
            {
                case ResultFromServer.Yes:

                    var result = await _itemsCollection.GetItemsFromServer();
                    if (result == ResultFromServer.Yes)
                    {

                        Task<ResultFromServer> task2 = _users.GetUserItemsGuid(_users.CurrentUser);
                        if (await task2 == ResultFromServer.Yes)
                        {

                            if (!_users.CurrentUser.IsAdmin)
                            {
                                LoginView.SetMainView();
                                _mainView.SetUserName(e.User.Username);
                            }
                            else
                            {
                                Task<ResultFromServer> task3 = _users.GetUsersFromServer();

                                if (await task3 == ResultFromServer.Yes)
                                {
                                    LoginView.SetMainView();
                                    _mainView.SetUserName(_users.CurrentUser.Username);
                                }
                                else
                                    LoginView.StringFromServer = "Error retriving users from server";
                            }

                        }
                        LoginView.StringFromServer = "Error retriving user items from server";
                    }
                    else
                        LoginView.StringFromServer = "Could not connect to server db";
                    break;
                case ResultFromServer.No:
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

        static BL()
        {
            _itemsCollection = new ItemsCollection();
        }
    }
}
