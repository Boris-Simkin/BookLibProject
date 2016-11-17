using Model;
using Presenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageUsersPage : Page, IManageUsers
    {
        public event EventHandler<SubmitEventArgs> MakeAdmin;
        public event EventHandler<SubmitEventArgs> DeleteUser;

        public ManageUsersPage()
        {
            this.InitializeComponent();
            this.Loaded += ManageUsersPage_Loaded;
        }

        public List<User> SourceList
        {
            get { return (List<User>)usersListView.ItemsSource; }
            set { usersListView.ItemsSource = value; }
        }

        private void ManageUsersPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainPresenter.ManageUsersPage = this;
           // usersListView.ItemsSource =;
        }

        private void deleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (DeleteUser != null)
            //    DeleteUser(this, new SubmitEventArgs());
        }

        private void makeAdminBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (MakeAdmin != null)
            //    MakeAdmin(this, new SubmitEventArgs());
        }
    }
}
