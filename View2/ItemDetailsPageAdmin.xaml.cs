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
    public sealed partial class ItemDetailsPageAdmin : Page, IItemDetailsPageAdmin
    {
        AbstractItem _item;

        public event EventHandler<ItemEventArgs> Save;
        public event EventHandler<ItemEventArgs> Delete;

        public ItemDetailsPageAdmin()
        {
            this.InitializeComponent();
            this.Loaded += ItemDetailsPageAdmin_Loaded;
        }

        private void ItemDetailsPageAdmin_Loaded(object sender, RoutedEventArgs e)
        {
            saveBtn.IsEnabled = false;
        }

        public void SetContent(AbstractItem item)
        {
            _item = item;
            string defaultImageLocation;
            string category;
            if (item is Book)
            {
                categoryCombobox.ItemsSource = Enum.GetValues(typeof(Book.BookCategory));
                categoryCombobox.SelectedItem = ((Book)item).Category;
                category = ((Book)item).Category.ToString();
                defaultImageLocation = "Assets/DefaultBookImage.png";
                deleteBtn.Content = "Delete book";
            }
            else
            {
                categoryCombobox.ItemsSource = Enum.GetValues(typeof(Journal.JournalCategory));
                categoryCombobox.SelectedItem = ((Journal)item).Category;
                category = ((Journal)item).Category.ToString();
                defaultImageLocation = "Assets/DefaultMagazineImage.png";
                deleteBtn.Content = "Delete magazine";
            }

            this.defaultCoverImage.Source =
                new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri($"ms-appx:///{defaultImageLocation}"));
            if (item.CoverImage != null && item.CoverImage != string.Empty)
            {
                image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri(item.CoverImage));
                coverImageTxtBox.Text = item.CoverImage;
            }
            GuidTxtBlk.Text = item.Guid.ToString();
            subCategoryTxtBox.Text = item.SubCategory;
            datePicker.Date = (DateTimeOffset)item.Date;
            datePicker.MaxYear = DateTime.Today;
            copyNumberTxtBox.Text = item.CopyNumber.ToString();
            itemNameTxtBox.Text = item.ItemName;
            borrowedСopiesTxtBlock.Text = item.BorrowedCopies.ToString();

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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPresenter.ItemDetailsPageAdmin = this;
        }

        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Delete != null)
                Delete(this, new ItemEventArgs(_item));
            if (_item is Book)
                Frame.Navigate(typeof(View.MessagePage), "Book is deleted.");
            else
                Frame.Navigate(typeof(View.MessagePage), "Magazine is deleted.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

            saveBtn.IsEnabled = false;
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
