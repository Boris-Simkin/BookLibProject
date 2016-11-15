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

        public ItemDetailsPage()
        {
            this.InitializeComponent();
        }

        public void SetContent(AbstractItem item)
        {
            string defaultImageLocation;
            string category;
            if (item is Book)
            {
                category = ((Book)item).Category.ToString();
                defaultImageLocation = "Assets/DefaultBookImage.png";
            }
            else
            {
                category = ((Journal)item).Category.ToString();
                defaultImageLocation = "Assets/DefaultMagazineImage.png";
            }

            this.defaultCoverImage.Source =
                new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri($"ms-appx:///{defaultImageLocation}"));
            if (item.CoverImage != null)
            image.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri(item.CoverImage));
            titleTxtBlk.Text = item.ItemName;
            GuidTxtBlk.Text = item.Guid.ToString();
            categoryTxtBlk.Text = category;
            subCategoryTxtBlk.Text = item.SubCategory;
            dateTxtBlk.Text = item.Date.ToString("d");
            avaliableСopiesTxtBlk.Text = (item.CopyNumber - item.BorrowedCopies).ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPresenter.ItemDetailsPage = this;
        }

        private void borrowBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
