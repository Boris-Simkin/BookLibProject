using Model;
using System;
using System.Collections.Generic;

namespace Presenter
{
    public interface IItemListPage
    {
        List<AbstractItem> SourceList { get; set; }
        bool IsBookList { get; set; }
        void SetItemDetailsPage(bool isAdmin);
        event EventHandler<ItemEventArgs> ItemClicked;
    }
}