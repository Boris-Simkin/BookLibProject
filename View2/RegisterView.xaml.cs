using Model;
using Presenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class RegisterView : Page, IRegisterView
    {
        public RegisterView()
        {
            this.InitializeComponent();
            this.Loaded += RegisterView_Loaded;
        }

        private void RegisterView_Loaded(object sender, RoutedEventArgs e)
        {
            MainPresenter.RegisterView = this;
        }

        public event EventHandler GoBack;
        public event EventHandler<UserEventArgs> Submit;

        public void RequestFinished()
        {
            submitBtn.IsEnabled = true;
            progressRing.IsActive = false;
            progressRing.IsEnabled = false;
        }

        public string StringFromServer { set { stringFromServer.Text = value; } }

        private bool satisfyConditions()
        {
            return ((usernameTxtBox.Text.Length >= 2) &&
                (firstNameTxtBox.Text.Length >= 2) &&
                (lastNameTxtBox.Text.Length >= 2) &&
                (passwordBox.Password.Length >= 4) &&
                (passwordBox.Password == confirmPasswordBox.Password));
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void cancleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GoBack != null)
                GoBack(this, EventArgs.Empty);
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            //Disabling the button while getting answer from the server
            submitBtn.IsEnabled = false;
            progressRing.IsActive = true;
            progressRing.IsEnabled = true;

            User user = new User
            {
                Username = usernameTxtBox.Text,
                Password = passwordBox.Password,
                FirstName = firstNameTxtBox.Text,
                LastName = lastNameTxtBox.Text
            };

            if (Submit != null)
                Submit(this, new UserEventArgs(user));
        }

        public async void ShowMessage(string message)
        {
            var messageDialog = new MessageDialog(message);
            await messageDialog.ShowAsync();
        }

        public void SetPreviusView()
        {
            Frame.GoBack();
        }
    }
}
