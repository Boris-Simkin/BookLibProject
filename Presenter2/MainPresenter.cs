using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    static public class MainPresenter
    {
        private static readonly IItemsCollection _itemsCollection;
        private static Users _user = new Users();

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
            }
        }


        private static void _loginView_Submit(object sender, SubmitEventArgs e)
        {
            LoginView.SetMainView(_itemsCollection.GetBooks(), _itemsCollection.GetJournals());
            _mainView.SetUserName(e.Username);
        }

        static MainPresenter()
        {
            _itemsCollection = new ItemsCollection();
            _itemsCollection.LoadData();
        }
    }
}
