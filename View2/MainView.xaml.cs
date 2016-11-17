﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Presenter;
using System.Diagnostics;
using Windows.UI;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page, IMainView
    {
        public event EventHandler Logout;
        public event EventHandler MagazinesClicked;
        public event EventHandler BooksClicked;
        public event EventHandler MyMagazinesClicked;
        public event EventHandler MyBooksClicked;
        public event EventHandler ManageUsersClicked;
        public event EventHandler<StringEventArgs> SearchTextChanged;

        public int SetCounter { set { itemCountTxtBlk.Text = value.ToString(); } }

        public void HideToolBar()
        {
            toolBarGrid.Visibility = Visibility.Collapsed;
        }

        public void ClearCounter()
        {
            itemCountTxtBlk.Text = string.Empty;
        }

        public void ClearTitle()
        {
            ClearCounter();
            titleTxtBlk.Text = string.Empty;
        }

        public Visibility AdminTools { get; set; }

        public void IsAdmin(bool value)
        {
            if (value)
                AdminTools = Visibility.Visible;
            else
                AdminTools = Visibility.Collapsed;
        }

        public MainView()
        {
            this.DataContext = this;
            this.InitializeComponent();
            this.Loaded += MainView_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPresenter.MainView = this;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            titleTxtBlk.Text = "Books";
            SetBooksListPage();
        }

        public void SetBooksListPage()
        {
            toolBarGrid.Visibility = Visibility.Visible;
            titleTxtBlk.Text = "Books";
            mainFrame.Navigate(typeof(ItemListPage));
            if (BooksClicked != null)
                BooksClicked(this, EventArgs.Empty);
        }

        public void SetMagazinesListPage()
        {
            toolBarGrid.Visibility = Visibility.Visible;
            titleTxtBlk.Text = "Magazines";
            mainFrame.Navigate(typeof(ItemListPage));
            if (MagazinesClicked != null)
                MagazinesClicked(this, EventArgs.Empty);
        }

        private void booksTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetBooksListPage();
        }

        private void magazinesTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SetMagazinesListPage();
        }

        private void myBooksTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            toolBarGrid.Visibility = Visibility.Visible;
            mainFrame.Navigate(typeof(ItemListPage));
            titleTxtBlk.Text = "My books";
            if (MyBooksClicked != null)
                MyBooksClicked(this, EventArgs.Empty);
        }

        private void SetListPage(EventHandler someEvent, string title)
        {

        }

        private void myMagazinesTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            toolBarGrid.Visibility = Visibility.Visible;
            mainFrame.Navigate(typeof(ItemListPage));
            titleTxtBlk.Text = "My magazines";
            if (MyMagazinesClicked != null)
                MyMagazinesClicked(this, EventArgs.Empty);
        }

        private void addNewItemTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            toolBarGrid.Visibility = Visibility.Collapsed;
            titleTxtBlk.Text = "Add new item";
            ClearCounter();
            mainFrame.Navigate(typeof(AddNewItemPage));
        }

        private void manageUsersTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            toolBarGrid.Visibility = Visibility.Collapsed;
            titleTxtBlk.Text = "Manage users";
            ClearCounter();
            HideToolBar();
            mainFrame.Navigate(typeof(ManageUsersPage));

            if (ManageUsersClicked != null)
                ManageUsersClicked(this, EventArgs.Empty);
 
        }

        public void SetUserName(string userName)
        {
            userNameTextBlock.Text = $"Hi, {userName}";
        }

        private void logoutTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (Logout != null)
                Logout(this, EventArgs.Empty);

            Frame.Navigate(typeof(LoginView));
        }

        private void searchTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            //titleTxtBlk.Text = "results";
            //ClearCounter();
        }

        private string _tempTitle;

        private void searchTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTxtBox.Text != string.Empty)
            {
                if (titleTxtBlk.Text != "Result")
                    _tempTitle = titleTxtBlk.Text;
                titleTxtBlk.Text = "Result";
                searchTextBlk.Visibility = Visibility.Collapsed;
            }
            else
            {
                searchTextBlk.Visibility = Visibility.Visible;
                titleTxtBlk.Text = _tempTitle;
            }
            if (SearchTextChanged != null)
                SearchTextChanged(this, new StringEventArgs(searchTxtBox.Text));

        }

        private void searchByBtn_Click(object sender, RoutedEventArgs e)
        {
            HideToolBar();
            ClearCounter();
            titleTxtBlk.Text = "Advanced search";
            mainFrame.Navigate(typeof(AdvancedSearchPage));
        }

        public void ShowMessage(string str)
        {
            mainFrame.Navigate(typeof(MessagePage), str);
        }
    }
}
