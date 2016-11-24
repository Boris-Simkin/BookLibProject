using Model;
using BookLibBL;
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
    public sealed partial class ItemDetailsPageAdmin : Page, IItemDetailsPageAdmin
    {
        AbstractItem _item;

        public event EventHandler<ItemEventArgs> Save;
        public event EventHandler<ItemEventArgs> Delete;
        public event EventHandler<ItemEventArgs> Borrow;

        public ItemDetailsPageAdmin()
        {
            this.InitializeComponent();
            this.Loaded += ItemDetailsPageAdmin_Loaded;
        }

        private void ItemDetailsPageAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            saveBtn.IsEnabled = false;
            DataContext = _item;
        }

        private void SetReading(bool isReading)
        {
            if (isReading)
            {
                borrowBtn.Content = "Return";
                // borrowedСopiesTxtBlock.Text = (int.Parse(borrowedСopiesTxtBlock.Text) + 1).ToString();
            }
            else
            {
                borrowBtn.Content = "Borrow";
                //    borrowedСopiesTxtBlock.Text = (int.Parse(borrowedСopiesTxtBlock.Text) - 1).ToString();
            }
        }


        public void SetContent(AbstractItem item, bool isReading)
        {
            _item = item;

            if (item.BorrowedCopies >= item.CopyNumber && !isReading)
            {
                noFreeCopiesTxtBlk.Visibility = Visibility.Visible;
                borrowBtn.IsEnabled = false;
            }
            string category;
            if (item is Book)
            {
                categoryCombobox.ItemsSource = Enum.GetValues(typeof(Book.BookCategory));
                categoryCombobox.SelectedItem = ((Book)item).Category;
                category = ((Book)item).Category.ToString();
                defaultCoverImage.Source = defaultBookImage.Source;
                deleteBtn.Content = "Delete book";
            }
            else
            {
                categoryCombobox.ItemsSource = Enum.GetValues(typeof(Journal.JournalCategory));
                categoryCombobox.SelectedItem = ((Journal)item).Category;
                category = ((Journal)item).Category.ToString();
                defaultCoverImage.Source = defaultMagazineImage.Source;
                deleteBtn.Content = "Delete magazine";
            }

            if (item.CoverImage != null)
                coverImageTxtBox.Text = item.CoverImage;

            GuidTxtBlk.Text = item.Guid.ToString();
            subCategoryTxtBox.Text = item.SubCategory;
            datePicker.MinYear = new DateTime(100, 1, 1);
            datePicker.MaxYear = DateTime.Today;
            datePicker.Date = (DateTimeOffset)item.Date;
            copyNumberTxtBox.Text = item.CopyNumber.ToString();
            itemNameTxtBox.Text = item.ItemName;
            borrowedСopiesTxtBlock.Text = item.BorrowedCopies.ToString();

            SetReading(isReading);
        }

        private void copyNumberTxtBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            e.Handled = Char.IsLetter((char)e.Key);
        }

        private void copyNumberTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int number;
            bool result = int.TryParse(copyNumberTxtBox.Text, out number);
            if (!result || (number < 1 || number < _item.BorrowedCopies))
                copyNumberTxtBox.Text = _item.BorrowedCopies > 1 ? _item.BorrowedCopies.ToString() : "1";

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BL.ItemDetailsPageAdmin = this;
        }

        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {
            borrowBtn.IsEnabled = false;
            if (Borrow != null)
                Borrow(this, new ItemEventArgs(_item));
        }

        private async void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog($"Are you sure, you want to delete this item?");
            messageDialog.Commands.Add(new UICommand("Yes", DeleteRequest));
            messageDialog.Commands.Add(new UICommand("No"));
            await messageDialog.ShowAsync();
        }

        private void DeleteRequest(IUICommand command)
        {
            if (Delete != null)
                Delete(this, new ItemEventArgs(_item));
        }

        public void SaveSucceeded()
        {
            saveBtn.IsEnabled = false;

            if (_item.CopyNumber > _item.BorrowedCopies)
            {
                noFreeCopiesTxtBlk.Visibility = Visibility.Collapsed;
                borrowBtn.IsEnabled = true;
            }

        }

        public void BorrowReturnSucceeded(bool isReading)
        {
            borrowBtn.IsEnabled = true;
            if (isReading)
                borrowedСopiesTxtBlock.Text = (int.Parse(borrowedСopiesTxtBlock.Text) + 1).ToString();
            else
                borrowedСopiesTxtBlock.Text = (int.Parse(borrowedСopiesTxtBlock.Text) - 1).ToString();
            SetReading(isReading);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //Is Book
            if (_item is Book)
                ((Book)_item).Category = (Book.BookCategory)categoryCombobox.SelectedItem;
            //Is Magazine
            else
                ((Journal)_item).Category = (Journal.JournalCategory)categoryCombobox.SelectedItem;

            _item.ItemName = itemNameTxtBox.Text;
            _item.SubCategory = subCategoryTxtBox.Text;
            _item.Date = datePicker.Date;
            _item.CopyNumber = int.Parse(copyNumberTxtBox.Text);
            _item.CoverImage = coverImageTxtBox.Text;

            if (Save != null)
                Save(this, new ItemEventArgs(_item));

        }

        private void PropChanged(object sender, TextChangedEventArgs e)
        {
            saveBtn.IsEnabled = itemNameTxtBox.Text != string.Empty;
        }

        private void PropChanged(object sender, SelectionChangedEventArgs e)
        {
            saveBtn.IsEnabled = true;
        }

        private void datePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            saveBtn.IsEnabled = true;
        }

    }
}
