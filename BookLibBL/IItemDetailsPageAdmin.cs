using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibBL
{
    public interface IItemDetailsPageAdmin
    {
        void SaveSucceeded();
        void SetContent(AbstractItem item, bool isReading);
        void BorrowReturnSucceeded(bool isReading);
        event EventHandler<ItemEventArgs> Save;
        event EventHandler<ItemEventArgs> Delete;
        event EventHandler<ItemEventArgs> Borrow;
    }
}
