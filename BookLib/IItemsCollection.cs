using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IItemsCollection
    {
        Task<AuthenticationResult> LoadData();
        void AddItem(AbstractItem item);
        void UpdateItem(AbstractItem newitem);
        void DeleteItem(AbstractItem item);
        AbstractItem GetItem();
        List<AbstractItem> SearchByName(bool IsBook, string itemName);
        List<AbstractItem> GetJournals();
        List<AbstractItem> GetBooks();
    }
}
