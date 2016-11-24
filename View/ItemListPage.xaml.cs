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
    public sealed partial class ItemListPage : Page, IItemListPage
    {
        public ItemListPage()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        public EnumListType ListType { get; set; }

        public event EventHandler<ItemEventArgs> ItemClicked;

        //public bool IsBookList { get; set; }
        public string defaultImageLocation
        {
            get
            {
                //Showing default image depending on item type
                if ((ListType & (EnumListType.Books | EnumListType.MyBooks)) != 0)
                    return "Assets/DefaultBookImage.png";
                else return "Assets/DefaultMagazineImage.png";
            }
        }

        public List<AbstractItem> SourceList
        {
            get { return (List<AbstractItem>)itemsGridView.ItemsSource; }
            set { itemsGridView.ItemsSource = value; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BL.Instance.ItemListPage = this;
        }

        private void itemsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ItemClicked != null)
                ItemClicked(this, new ItemEventArgs((AbstractItem)e.ClickedItem));
        }

        public void SetItemDetailsPage(bool isAdmin)
        {
            if (isAdmin)
                Frame.Navigate(typeof(ItemDetailsPageAdmin));
            else
                Frame.Navigate(typeof(ItemDetailsPage));
        }

    }
}
