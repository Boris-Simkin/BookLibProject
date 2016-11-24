using Model;
using Presenter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class AddNewItemPage : Page, IAddNewItemPage
    {
        public AddNewItemPage()
        {
            this.InitializeComponent();
            this.Loaded += AddNewItemPage_Loaded;
        }

        Guid _newGuid = Guid.NewGuid();


        private void AddNewItemPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainPresenter.AddNewItemPage = this;
            typeCombobox.Items.Add("Books");
            typeCombobox.Items.Add("Magazines");
            typeCombobox.SelectedIndex = 0;
            GuidTxtBlk.Text = _newGuid.ToString();
            datePicker.MinYear = new DateTime(100, 1, 1);
            datePicker.MaxYear = DateTime.Today;
            categoryCombobox.ItemsSource = Enum.GetValues(typeof(Book.BookCategory));
            categoryCombobox.SelectedIndex = 0;
        }

        private bool SatisfyConditions()
        {
            return itemNameTxtBox.Text != string.Empty;
        }

        public event EventHandler<ItemEventArgs> Submit;

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AbstractItem _newItem;
            //Is Book
            if (typeCombobox.SelectedIndex == 0)
            {
                _newItem = new Book(itemNameTxtBox.Text, new Guid(GuidTxtBlk.Text),
                    (Book.BookCategory)categoryCombobox.SelectedItem, subCategoryTxtBox.Text);
            }
            //Is Magazine
            else
            {
                _newItem = new Journal(itemNameTxtBox.Text, new Guid(GuidTxtBlk.Text),
                    (Journal.JournalCategory)categoryCombobox.SelectedItem, subCategoryTxtBox.Text);
            }

            _newItem.Date = datePicker.Date;
            _newItem.CopyNumber = int.Parse(copyNumberTxtBox.Text);
            _newItem.CoverImage = coverImageTxtBox.Text;

            if (Submit != null)
                Submit(this, new ItemEventArgs(_newItem));
        }

        private void typeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typeCombobox.SelectedItem.ToString() == "Books")
            {
                categoryCombobox.ItemsSource = Enum.GetValues(typeof(Book.BookCategory));
                categoryCombobox.SelectedIndex = 0;
            }
            else
            {
                categoryCombobox.ItemsSource = Enum.GetValues(typeof(Journal.JournalCategory));
                categoryCombobox.SelectedIndex = 0;
            }
        }

        private void copyNumberTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = Char.IsLetter((char)e.Key);
        }

        private void copyNumberTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int number;
            bool result = int.TryParse(copyNumberTxtBox.Text, out number);
            if (!result || number < 1)
                copyNumberTxtBox.Text = "1";
        }

        private void itemNameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddBtn.IsEnabled = SatisfyConditions();
        }
    }
}
