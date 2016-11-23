using Model;
using Presenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginView : Page, ILoginView
    {
        public LoginView()
        {
            this.InitializeComponent();
            this.Loaded += LoginView_Loaded;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
             MainPresenter.LoginView = this;
        }

        public string StringFromServer { set { stringFromServer.Text = value; } }

        public event EventHandler registerBtnClick;
        public event EventHandler<UserEventArgs> Submit;

        public void RequestFinished()
        {
            progressRing.IsActive = false;
            progressRing.IsEnabled = false;
            submitBtn.IsEnabled = true;
        }

        public void SetMainView()
        {
            Frame.Navigate(typeof(View.MainView));
        }

        private bool satisfyConditions()
        {
            submitBtn.IsEnabled = true;
            return ((usernameTxtBox.Text.Length > 0) && (passwordBox.Password.Length > 0));
        }

        private void usernameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            submitBtn.IsEnabled = satisfyConditions();
            //Clear the string message if exist
            stringFromServer.Text = string.Empty;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            submitBtn.IsEnabled = satisfyConditions();
            //Clear the string message if exist
            stringFromServer.Text = string.Empty;
        }

        private void passwordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(VirtualKey.Enter))
            {
                //Prevent the key pressed twice
                e.Handled = true;
                SendSubmitParams();
            }
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            SendSubmitParams();
        }

        private void SendSubmitParams()
        {
            //Disabling the button while getting answer from the server
            submitBtn.IsEnabled = false;
            progressRing.IsActive = true;
            progressRing.IsEnabled = true;
            User user = new User
            {
                Username = usernameTxtBox.Text,
                Password = passwordBox.Password
            };
            if (Submit != null)
                Submit(this, new UserEventArgs(user));
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (registerBtnClick != null)
                registerBtnClick(this, EventArgs.Empty);
        }

        public void SetRegistrationView()
        {
            Frame.Navigate(typeof(RegisterView));
        }
    }
}
