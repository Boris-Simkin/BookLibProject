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
    public sealed partial class AdvancedSearchPage : Page, IAdvancedSearchPage
    {
        public AdvancedSearchPage()
        {
            this.InitializeComponent();
            this.Loaded += AdvancedSearchPage_Loaded;
        }

        AbstractItem _item;

        public event EventHandler<ItemEventArgs> Submit;

        private void AdvancedSearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainPresenter.AdvancedSearchPage = this;

            searchBtn.IsEnabled = false;

            datePicker.MaxYear = DateTime.Today;
            categoryCombobox.ItemsSource = Enum.GetValues(typeof(Book.BookCategory));
            categoryCombobox.SelectedIndex = 0;
        }

        private bool satisfyConditions()
        {
            return itemNameTxtBox.IsEnabled || categoryCombobox.IsEnabled
          || subCategoryTxtBox.IsEnabled || datePicker.IsEnabled;
        }


        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            searchBtn.IsEnabled = satisfyConditions();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {

            //Is Book
     //       if (typeCombobox.SelectedIndex == 0)
            {
                _item = new Book(itemNameTxtBox.Text, new Guid(),
                    (Book.BookCategory)categoryCombobox.SelectedItem, subCategoryTxtBox.Text);
                if (categoryCheckBox.IsChecked == false)
                    ((Book)_item).Category = null;
            }
          //  //Is Magazine
          ////  else
          //  {
          //      _item = new Journal(itemNameTxtBox.Text, new Guid(),
          //          (Journal.JournalCategory)categoryCombobox.SelectedItem, subCategoryTxtBox.Text);
          //      if (categoryCheckBox.IsChecked == false)
          //          ((Journal)_item).Category = null;
          //  }

            if (dateCheckBox.IsChecked == true)
                _item.Date = datePicker.Date;
            else _item.Date = null;

            if (itemNameTxtBox.Text == string.Empty || itemNameCheckBox.IsChecked == false)
                _item.ItemName = null;

            Frame.Navigate(typeof(ItemListPage));


            if (Submit != null)
                Submit(this, new ItemEventArgs(_item));
        }
    }
}
