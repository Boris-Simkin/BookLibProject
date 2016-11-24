using Model;
using BookLibBL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ManageUsersPage : Page, IManageUsers
    {
        public event EventHandler<UserEventArgs> MakeAdmin;
        public event EventHandler<UserEventArgs> DeleteUser;
        public event EventHandler PageLoaded;

        public ManageUsersPage()
        {
            this.InitializeComponent();
            this.Loaded += ManageUsersPage_Loaded;
        }

        public void RequestFinished(bool removeFromTable)
        {
            makeAdminBtn.IsEnabled = true;
            deleteUserBtn.IsEnabled = true;
            if (removeFromTable)
                sourceList.Remove((User)usersListView.SelectedItem);
        }

        private ObservableCollection<User> sourceList { get; set; }

        public List<User> SourceList
        {
            set { sourceList = new ObservableCollection<User>(value); }
        }


        private void ManageUsersPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            BL.Instance.ManageUsersPage = this;
            if (PageLoaded != null)
                PageLoaded(this, EventArgs.Empty);

            usersListView.ItemsSource = sourceList;
        }

        private async void deleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog($"Are you sure, you want to delete {((User)usersListView.SelectedItem).FirstName}?");
            messageDialog.Commands.Add(new UICommand("Yes", DeleteRequest));
            messageDialog.Commands.Add(new UICommand("No"));
            await messageDialog.ShowAsync();
        }

        private async void makeAdminBtn_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog($"Are you sure, you want make {((User)usersListView.SelectedItem).FirstName} administrator?");
            messageDialog.Commands.Add(new UICommand("Yes", MakeAdminRequest));
            messageDialog.Commands.Add(new UICommand("No"));
            await messageDialog.ShowAsync();
        }

        private void DeleteRequest(IUICommand command)
        {
            deleteUserBtn.IsEnabled = false;
            var user = (User)usersListView.SelectedItem;
            if (DeleteUser != null)
                DeleteUser(this, new UserEventArgs(user));
        }

        private void MakeAdminRequest(IUICommand command)
        {
            makeAdminBtn.IsEnabled = false;
            var user = (User)usersListView.SelectedItem;
            if (MakeAdmin != null)
                MakeAdmin(this, new UserEventArgs(user));
        }

        private void usersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            makeAdminBtn.IsEnabled = usersListView.SelectedItem != null;
            deleteUserBtn.IsEnabled = usersListView.SelectedItem != null;
        }
    }
}
