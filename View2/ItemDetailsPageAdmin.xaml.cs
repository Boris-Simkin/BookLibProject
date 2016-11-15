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

        public ItemDetailsPageAdmin()
        {
            this.InitializeComponent();
        }

        public void SetContent(AbstractItem item)
        {
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
            if (item.CoverImage != null)
            {
                image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri(item.CoverImage));
                coverImageTxtBox.Text = item.CoverImage;
            }
            GuidTxtBlk.Text = item.Guid.ToString();
            subCategoryTxtBox.Text = item.SubCategory;
            datePicker.Date = item.Date;
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

    }
}
