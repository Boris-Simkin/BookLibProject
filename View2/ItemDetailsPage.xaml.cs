using Model;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AbstractItem item = e.Parameter as AbstractItem;


            if (item is Book)
            {
                GuidTxtBlk.Text = ((Book)item).ISBN.ToString();
                categoryTxtBlk.Text = ((Book)item).Category.ToString();
                subCategoryTxtBlk.Text = ((Book)item).SubCategory;
             }
            else
            {
                GuidTxtBlk.Text = ((Journal)item).ISSN.ToString();
                categoryTxtBlk.Text = ((Journal)item).Category.ToString();
                subCategoryTxtBlk.Text = ((Journal)item).SubCategory;
            }

            defaultCoverImage.Source =
                 new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri($"ms-appx://{item.DefaultCoverImage}"));

            titleTxtBlk.Text = item.ItemName;
            if (item.CoverImage != null)
                coverImage.Source =
                    new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri(item.CoverImage));
            dateTxtBlk.Text = item.Date.ToString();


        }
    }
}
