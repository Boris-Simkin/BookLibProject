﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IItemsCollection
    {
        Task<ResultFromServer> LoadDataFromServer();
        Task<ResultFromServer> AddItemToServer(AbstractItem item);
        Task<ResultFromServer> DeleteFromServer(AbstractItem item);
        Task<ResultFromServer> UpdateInServer(AbstractItem item);
        void AddItem(AbstractItem item);
        void UpdateItem(AbstractItem newitem);
        void DeleteItem(AbstractItem item);
        AbstractItem GetItem();
        List<AbstractItem> SearchByName(bool IsBook, string itemName);
        List<AbstractItem> AdvancedSearch(AbstractItem item);
        List<AbstractItem> GetJournals();
        List<AbstractItem> GetBooks();
        void ClearList();
    }
}
