using Model;
using System;
using System.Collections.Generic;

namespace BookLibBL
{
    public interface IItemListPage
    {
        List<AbstractItem> SourceList { get; set; }
        EnumListType ListType { get; set; }
        //bool IsBookList { get; set; }
        void SetItemDetailsPage(bool isAdmin);
        event EventHandler<ItemEventArgs> ItemClicked;
    }
}