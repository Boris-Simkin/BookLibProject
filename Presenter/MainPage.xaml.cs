//#undef TestingMode
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using View;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Presenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IItemsCollection _itemsCollection = new ItemsCollection();
        private readonly Users _user = new Users();

        public MainPage()
        {
            this.Loaded += MainPage_Loaded;
        }

        private void SetLoginView()
        {
            Frame.Navigate(typeof(View.LoginView));
            Views.LoginView.registerBtnClick += View_registerBtnClick;
            Views.LoginView.Submit += LoginView_Submit;
        }


        private void SetRegisterView()
        {
            Frame.Navigate(typeof(View.RegisterView));
            Views.RegisterView.Submit += RegisterView_Submit;
            Views.RegisterView.GoBack += GoBackToLoginView;
            //Views.LoginCreatedPage.GoBack += GoBackToLoginView;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLoginView();
        }

        private void View_registerBtnClick(object sender, EventArgs e)
        {
            SetRegisterView();
        }

        private async void RegisterView_Submit(object sender, SubmitEventArgs e)
        {
            Task<AuthenticationResult> task = _user.Registration(e.Username, e.Password, e.Firstname, e.Lastname);
            switch (await task)
            {
                case AuthenticationResult.Ok:
                    Views.RegisterView.SetLoginCreatedPage();
                    break;
                case AuthenticationResult.ParamsIncorrect:
                    Views.RegisterView.StringFromServer = "This username already in use";
                    break;
                case AuthenticationResult.ConnectionFailed:
                    Views.RegisterView.StringFromServer = "Error connecting to the server";
                    break;
                default:
                    break;
            }
        }

#if TestingMode
        private void LoginView_Submit(object sender, SubmitEventArgs e)
        {
            _itemsCollection.LoadData();
            Frame.Navigate(typeof(View.MainView));
            Views.MainView.BooksSource = _itemsCollection.GetBooks();
            Views.MainView.JournalsSource = _itemsCollection.GetJournals();
            Views.MainView.UserName = e.Username;

        }
#endif
#if !TestingMode
        private async void LoginView_Submit(object sender, SubmitEventArgs e)
        {

            //await _itemsCollection.LoadData();

            Task<AuthenticationResult> task = _user.Authentication(e.Username, e.Password);

            //task = AuthenticationResult.Ok;
            switch (await task)
            {
                case AuthenticationResult.Ok:
                    await _itemsCollection.LoadData();
                    Frame.Navigate(typeof(View.MainView));


                    Views.MainView.BooksSource = _itemsCollection.GetBooks();
                    Views.MainView.JournalsSource = _itemsCollection.GetJournals();
                    Views.MainView.UserName = e.Username;
                    break;
                case AuthenticationResult.ParamsIncorrect:
                    Views.LoginView.StringFromServer = "Username or Password is incorrect!";
                    break;
                case AuthenticationResult.ConnectionFailed:
                    Views.LoginView.StringFromServer = "Error connecting to the server";
                    break;
                default:
                    break;
            }

    }
#endif
        private void GoBackToLoginView(object sender, EventArgs e)
        {
            SetLoginView();
        }
    }
}
