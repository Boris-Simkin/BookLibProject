using System;
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
using Presenter2;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace View2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page, IMainView, INotifyPropertyChanged
    {
        List<AbstractItem> booksSource;

        List<AbstractItem> journalsSource;

        public event PropertyChangedEventHandler PropertyChanged;


        //public List<AbstractItem> BooksSource
        //{
        //    set
        //    {
        //        booksSource = value;
        //    }
        //}


        //public List<AbstractItem> JournalsSource
        //{
        //    set
        //    {
        //        journalsSource = value;
        //    }
        //}

        //public MainView()
        //{
        //    this.InitializeComponent();
        //    Views.RegisterView = this;
        //}

        public MainView()
        {
            this.DataContext = this;
            this.InitializeComponent();
            this.Loaded += MainView_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPresenter.MainView = this;
            booksSource = ((List<AbstractItem>[])e.Parameter)[0];
            journalsSource = ((List<AbstractItem>[])e.Parameter)[1];
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            titleTxtBlk.Text = "Books";
            itemCountTxtBlk.Text = $"({booksSource.Count})";
            mainFrame.Navigate(typeof(ItemListPage), booksSource);
        }

        //public string UserName
        //{
        //    set { userNameTextBlock.Text = $"Hi, {value}"; }
        //}


        string _currentItemCount;
        public string CurrentItemCount
        {
            get { return _currentItemCount; }
            set
            {
                if (value != this._currentItemCount)
                {
                    this._currentItemCount = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Itemcount"));
                    }
                }
            }
        }
        private void booksTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            titleTxtBlk.Text = "Books";
            _currentItemCount = booksSource.Count.ToString();
            itemCountTxtBlk.Text = $"({booksSource.Count})";
            mainFrame.Navigate(typeof(ItemListPage), booksSource);
        }

        private void magazinesTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            titleTxtBlk.Text = "Magazines";
            _currentItemCount = journalsSource.Count.ToString();
            itemCountTxtBlk.Text = $"({journalsSource.Count})";
            mainFrame.Navigate(typeof(ItemListPage), journalsSource);
        }

        private void myItemsTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            titleTxtBlk.Text = "My books";
        }

        private void addNewItemTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            titleTxtBlk.Text = "Add new item";
        }

        private void manageUsersTxtBlk_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            titleTxtBlk.Text = "Manage users";
        }

        public void SetUserName(string userName)
        {
            userNameTextBlock.Text = $"Hi, {userName}";
        }
    }
}
