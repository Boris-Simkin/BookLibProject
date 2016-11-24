using Model;
using System;

namespace BookLibBL
{
    public interface IItemDetailsPage
    {
        void SetContent(AbstractItem item, bool isReading);
        void BorrowReturnSucceeded(bool isReading);
        event EventHandler<ItemEventArgs> Borrow;
    }
}