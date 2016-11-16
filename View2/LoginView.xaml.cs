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
        public event EventHandler<SubmitEventArgs> Submit;
        

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
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            submitBtn.IsEnabled = satisfyConditions();
        }

        private void passwordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(VirtualKey.Enter))
            {
                //Prevent the key pressed twice
                e.Handled = true;

                if (Submit != null)
                    Submit(this, new SubmitEventArgs(usernameTxtBox.Text, passwordBox.Password));
            }
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Submit != null)
                Submit(this, new SubmitEventArgs(usernameTxtBox.Text, passwordBox.Password));
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
