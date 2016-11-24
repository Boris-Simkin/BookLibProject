using Model;
using BookLibBL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class ItemDetailsPage : Page, IItemDetailsPage
    {
        AbstractItem _item;

        public ItemDetailsPage()
        {
            this.InitializeComponent();
        }

        public event EventHandler<ItemEventArgs> Borrow;

        private void SetReading(bool isReading)
        {
            if (isReading)
                borrowBtn.Content = "Return";
            else
                borrowBtn.Content = "Borrow";
        }

        public void BorrowReturnSucceeded(bool isReading)
        {
            if (isReading)
                avaliableСopiesTxtBlk.Text = (int.Parse(avaliableСopiesTxtBlk.Text) - 1).ToString();
            else
                avaliableСopiesTxtBlk.Text = (int.Parse(avaliableСopiesTxtBlk.Text) + 1).ToString();

            borrowBtn.IsEnabled = true;
            SetReading(isReading);
        }

        public void SetContent(AbstractItem item, bool isReading)
        {
            _item = item;
            if (item.BorrowedCopies >= item.CopyNumber && !isReading)
            {
                noFreeCopiesTxtBlk.Visibility = Visibility.Visible;
                borrowBtn.IsEnabled = false;
            }
            DataContext = item;
            string category;
            //Setting default images for items without covers
            if (item is Book)
            {
                category = ((Book)item).Category.ToString();
                defaultCoverImage.Source = defaultBookImage.Source;
            }
            else
            {
                category = ((Journal)item).Category.ToString();
                defaultCoverImage.Source = defaultMagazineImage.Source;
            }

            titleTxtBlk.Text = item.ItemName;
            GuidTxtBlk.Text = item.Guid.ToString();
            categoryTxtBlk.Text = category;
            subCategoryTxtBlk.Text = item.SubCategory;
            dateTxtBlk.Text = ((DateTimeOffset)item.Date).ToString("d");
            avaliableСopiesTxtBlk.Text = (item.CopyNumber - item.BorrowedCopies).ToString();

            SetReading(isReading);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BL.ItemDetailsPage = this;
        }

        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {
            borrowBtn.IsEnabled = false;
            if (Borrow != null)
                Borrow(this, new ItemEventArgs(_item));
        }
    }
}
