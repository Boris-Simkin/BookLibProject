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
        AbstractItem GetItem();
        List<AbstractItem> GetJournals();
        List<AbstractItem> GetBooks();
    }
}
