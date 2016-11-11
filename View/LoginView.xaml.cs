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
        // Views _views = Views.Instance;

        public LoginView()
        {
            this.InitializeComponent();
            Views.LoginView = this;
            //Users users = new Users();
            //Login login = new Login(this, users);
        }

        //public event EventHandler<SubmitEventArgs> Submit;
        public string StringFromServer { set { stringFromServer.Text = value; } }

        public event EventHandler registerBtnClick;
        public event EventHandler<SubmitEventArgs> Submit;

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
            // _views.LoginView = null;

            //Frame.Navigate(typeof(RegisterView), _views);

            if (registerBtnClick != null)
                registerBtnClick(this, EventArgs.Empty);
        }



    }
}
