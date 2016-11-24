//#define OfflineMode
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ItemsCollection : IItemsCollection
    {
        private List<AbstractItem> _items = new List<AbstractItem>();

        public bool IsEmpty()
        {
            return !_items.Any();
        }

        public void AddItem(AbstractItem item)
        {
            _items.Add(item);
        }


        public List<AbstractItem> SearchByName(EnumListType listType, string itemName, User user)
        {
            switch (listType)
            {
                case EnumListType.Books:
                    return new List<AbstractItem>(GetBooks().Where(p => p.ItemName.ToLower().Contains(itemName.ToLower())));
                case EnumListType.Magazines:
                    return new List<AbstractItem>(GetJournals().Where(p => p.ItemName.ToLower().Contains(itemName.ToLower())));
                case EnumListType.MyBooks:
                    return new List<AbstractItem>(GetUserBooks(user).Where(p => p.ItemName.ToLower().Contains(itemName.ToLower())));
                case EnumListType.MyMagazines:
                    return new List<AbstractItem>(GetUserJournals(user).Where(p => p.ItemName.ToLower().Contains(itemName.ToLower())));

            }
            return null;
            //if (IsBook)
            //    return new List<AbstractItem>(_items.Where(p => p.ItemName.ToLower().Contains(itemName.ToLower()) && p is Book));
            //else
            //    return new List<AbstractItem>(_items.Where(p => p.ItemName.ToLower().Contains(itemName.ToLower()) && p is Journal));
        }


        public List<AbstractItem> AdvancedSearch(AbstractItem item)
        {
            //return new List<AbstractItem>(_items.Where(i => i is Book));
            List<AbstractItem> list;
            if (item is Book)
                list = GetBooks();
            else
                list = GetJournals();

            return new List<AbstractItem>(list.Where(i => i.ItemName.ToLower().Contains(item.ItemName.ToLower())
            && i.SubCategory.ToLower().Contains(item.SubCategory.ToLower())));
        }

        public List<AbstractItem> GetBooks()
        {
            return new List<AbstractItem>(_items.Where(item => item is Book));
        }

        public List<AbstractItem> GetJournals()
        {
            return new List<AbstractItem>(_items.Where(item => item is Journal));
        }

        public List<AbstractItem> GetUserBooks(User user)
        {
            return new List<AbstractItem>(GetBooks().Where(item => user.MyItems.Contains(item.Guid)));
        }

        public List<AbstractItem> GetUserJournals(User user)
        {
            return new List<AbstractItem>(GetJournals().Where(item => user.MyItems.Contains(item.Guid)));
        }

        public void UpdateItem(AbstractItem newitem)
        {
            AbstractItem selectedItem = _items.First(item => item.Guid == newitem.Guid);
            selectedItem = newitem;
        }

        /// <summary>
        /// Return the items which borrowed by user
        /// </summary>
        /// <param name="user"></param>
        public void ReturnAllUserItems(User user)
        {
            //Decrease by one the BorrowedCopies property in each item which was owned by the user
            foreach (var item in _items.Where(item => user.MyItems.Contains(item.Guid)))
                item.BorrowedCopies--;
            //Clear the list of user's items
            user.MyItems.Clear();
        }

        public void DeleteItem(AbstractItem item)
        {
            _items.Remove(item);
        }

        public void ClearList()
        {
            _items.Clear();
        }

        #region Server management methods
        /// <summary>
        /// Borrow or return the item of the user on the server
        /// </summary>
        /// <param name="user"></param>
        /// <param name="item"></param>
        /// <param name="borrow">Borrow or return</param>
        /// <returns></returns>
        public async Task<ResultFromServer> BorrowReturnServer(User user, AbstractItem item, bool borrow)
        {
            var values = new Dictionary<string, string>();
            values.Add("Guid", item.Guid.ToString());
            values.Add("Username", user.Username);

            if (borrow)
                return await Server.Connect("Borrow.php", values);
            else
                return await Server.Connect("Return.php", values);
        }

        /// <summary>
        /// Update all the specific item in the server
        /// </summary>
        /// <param name="item">The item to update</param>
        /// <returns></returns>
        public async Task<ResultFromServer> UpdateItemInServer(AbstractItem item)
        {
            var values = new Dictionary<string, string>();
            values.Add("BookName", item.ItemName);
            values.Add("Date", ((DateTimeOffset)item.Date).ToString("d"));
            values.Add("CopyNumber", item.CopyNumber.ToString());
            values.Add("Guid", item.Guid.ToString());
            // The url should be encrypted, otherwise the server will prohibit store links
            values.Add("CoverImage", Crypt.Encrypt("boris", item.CoverImage));
            if (item is Book)
                values.Add("Category", ((Book)item).Category.ToString());
            else
                values.Add("Category", ((Journal)item).Category.ToString());
            values.Add("SubCategory", item.SubCategory);
            values.Add("BorrowedCopies", item.BorrowedCopies.ToString());

            return await Server.Connect("UpdateBook.php", values);
        }

        /// <summary>
        /// Delete an item from the server
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> DeleteFromServer(AbstractItem item)
        {
            var values = new Dictionary<string, string>();
            values.Add("Guid", item.Guid.ToString());
            return await Server.Connect("DeleteBook.php", values);
        }

        /// <summary>
        /// Add item to the server
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> AddItemToServer(AbstractItem item)
        {
            var values = new Dictionary<string, string>();
            values.Add("BookName", item.ItemName);
            values.Add("Date", ((DateTimeOffset)item.Date).ToString("d"));
            values.Add("CopyNumber", item.CopyNumber.ToString());
            values.Add("Guid", item.Guid.ToString());
            // The url should be encrypted, otherwise the server will prohibit store links
            values.Add("CoverImage", Crypt.Encrypt("boris", item.CoverImage));
            if (item is Book)
                values.Add("Category", ((Book)item).Category.ToString());
            else
                values.Add("Category", ((Journal)item).Category.ToString());
            values.Add("SubCategory", item.SubCategory);
            values.Add("BorrowedCopies", item.BorrowedCopies.ToString());

            if (await Server.Connect("AddBook.php", values) != ResultFromServer.Yes)
                return ResultFromServer.ConnectionFailed;

            return ResultFromServer.Yes;
        }

        /// <summary>
        /// Get the items from the server
        /// </summary>
        /// <returns></returns>
        public async Task<ResultFromServer> GetItemsFromServer()
        {
            var result = await Server.Connect("GetBooks.php");
            if (result != ResultFromServer.ConnectionFailed)
            {
                // The retrieved content from the server
                string[] words = Server.ResponseWords;

                for (int i = 0; i < (words.Length - 8); i += 8)
                {
                    AbstractItem newItem;
                    Book.BookCategory bookCategory;
                    Journal.JournalCategory journalCategory;
                    //Trying parse the category to book or journal
                    bool isbook = Enum.TryParse(words[i + 5], out bookCategory);
                    Enum.TryParse(words[i + 5], out journalCategory);

                    if (isbook)
                        newItem = new Book(words[i + 0], new Guid(words[i + 3]), bookCategory, words[i + 6]);
                    else
                        newItem = new Journal(words[i + 0], new Guid(words[i + 3]), journalCategory, words[i + 6]);

                    newItem.Date = DateTimeOffset.Parse(words[i + 1], CultureInfo.InvariantCulture);

                    //Decrypt the url
                    newItem.CoverImage = Crypt.Decrypt("boris", words[i + 4]);

                    newItem.CopyNumber = int.Parse(words[i + 2]);
                    newItem.BorrowedCopies = int.Parse(words[i + 7]);
                    _items.Add(newItem);

                }
                return ResultFromServer.Yes;
            }
            return result;
        }
        #endregion

    }
}
