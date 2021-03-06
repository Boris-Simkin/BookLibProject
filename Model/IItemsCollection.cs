﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IItemsCollection
    {
        #region Server management methods
        Task<ResultFromServer> GetItemsFromServer();
        Task<ResultFromServer> AddItemToServer(AbstractItem item);
        Task<ResultFromServer> DeleteFromServer(AbstractItem item);
        Task<ResultFromServer> UpdateItemInServer(AbstractItem item);
        Task<ResultFromServer> BorrowReturnServer(User user, AbstractItem item, bool borrow);
        #endregion
        List<AbstractItem> SearchByName(EnumListType listType, string itemName, User user);
        List<AbstractItem> AdvancedSearch(AbstractItem item);
        List<AbstractItem> GetUserJournals(User user);
        List<AbstractItem> GetUserBooks(User user);
        List<AbstractItem> GetJournals();
        List<AbstractItem> GetBooks();
        void AddItem(AbstractItem item);
        void UpdateItem(AbstractItem newitem);
        void DeleteItem(AbstractItem item);
        void ReturnAllUserItems(User user);
        void ClearList();
    }
}
