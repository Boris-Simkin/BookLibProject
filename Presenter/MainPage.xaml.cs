using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            Views.RegisterView.submit += RegisterView_submit;
            Views.RegisterView.goBack += RegisterView_goBack;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

            SetLoginView();
        }

        private void View_registerBtnClick(object sender, EventArgs e)
        {
            SetRegisterView();
        }

        private void RegisterView_submit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoginView_Submit(object sender, SubmitEventArgs e)
        {
            
            Frame.Navigate(typeof(View.MainView));
            _itemsCollection.LoadData();
            Views.MainView.BooksSource = _itemsCollection.GetBooks();
            Views.MainView.JournalsSource = _itemsCollection.GetJournals();
            Views.MainView.UserName = e.Username;
        }

        private void RegisterView_goBack(object sender, EventArgs e)
        {
            SetLoginView();
        }
    }
}
