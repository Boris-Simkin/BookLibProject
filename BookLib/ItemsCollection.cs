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

        public ItemsCollection()
        {
        }

        public List<AbstractItem> _items = new List<AbstractItem>();

        public bool IsEmpty()
        {
            return !_items.Any();
        }

        public void AddItem(AbstractItem item)
        {
            _items.Add(item);
        }

        public AbstractItem GetItem()
        {
            throw new NotImplementedException();
        }

        public List<AbstractItem> SearchByName(bool IsBook, string itemName)
        {
            if (IsBook)
                return new List<AbstractItem>(_items.Where(p => p.ItemName.Contains(itemName) && p is Book));
            else
                return new List<AbstractItem>(_items.Where(p => p.ItemName.Contains(itemName) && p is Journal));

        }

        public List<AbstractItem> AdvancedSearch(AbstractItem item)
        {
            throw new NotImplementedException();
        }

        public List<AbstractItem> GetBooks()
        {
            return new List<AbstractItem>(_items.Where(item => item is Book));
        }

        public List<AbstractItem> GetJournals()
        {
            return new List<AbstractItem>(_items.Where(item => item is Journal));
        }

        public void UpdateItem(AbstractItem newitem)
        {
            AbstractItem selectedItem = _items.First(item => item.Guid == newitem.Guid);
            selectedItem = newitem;
        }

        public void DeleteItem(AbstractItem item)
        {
            _items.Remove(item);
        }

        public void ClearList()
        {
            _items.Clear();
        }

        public async Task<AuthenticationResult> AddItemToServer(AbstractItem item)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            AuthenticationResult result = AuthenticationResult.ConnectionFailed;

            var values = new Dictionary<string, string>();
            values.Add("BookName", item.ItemName);
            values.Add("Date", ((DateTimeOffset)item.Date).ToString("d"));
            values.Add("CopyNumber", item.CopyNumber.ToString());
            values.Add("Guid", item.Guid.ToString());
            values.Add("CoverImage", item.CoverImage);
            if (item is Book)
                values.Add("Category", ((Book)item).Category.ToString());
            else
                values.Add("Category", ((Journal)item).Category.ToString());
            values.Add("SubCategory", item.SubCategory);
            values.Add("BorrowedCopies", item.BorrowedCopies.ToString());
            var content = new FormUrlEncodedContent(values);

            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/AddBook.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(responseBody);
                    if (responseBody == "created")
                        result = AuthenticationResult.Ok;
                }
                else
                    result = AuthenticationResult.ConnectionFailed;
            }
            catch
            {
                result = AuthenticationResult.ConnectionFailed;
            }
            return result;
        }



#if !OfflineMode
        public async Task<AuthenticationResult> LoadDataFromServer()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            //AuthenticationResult result = AuthenticationResult.Ok;
            string responseBody = string.Empty;
            try
            {
                response = await httpClient.GetAsync("http://simkin.atwebpages.com/GetBooks.php");
                if (response.StatusCode == HttpStatusCode.OK)
                    responseBody = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return AuthenticationResult.ConnectionFailed;
            }


            string[] words = responseBody.Split('^');
            for (int i = 0; i < (words.Length - 8); i += 8)
            {
                Debug.WriteLine("i: " + i);
                AbstractItem newItem;
                Book.BookCategory bookCategory;
                Journal.JournalCategory journalCategory;
                //Trying parse the category to book or journal
                bool isbook = Enum.TryParse(words[i + 5], out bookCategory);
                Enum.TryParse(words[5], out journalCategory);

                if (isbook)
                    newItem = new Book(words[i + 0], new Guid(words[i + 3]), bookCategory, words[i + 6]);
                else
                    newItem = new Journal(words[i + 0], new Guid(words[i + 3]), journalCategory, words[i + 6]);

                Debug.WriteLine(words[i + 1]);
                Debug.WriteLine(words[i + 1]);
                newItem.Date = DateTimeOffset.Parse(words[i + 1], CultureInfo.InvariantCulture);


                newItem.CoverImage = words[i + 4];
                newItem.CopyNumber = int.Parse(words[i + 2]);
                newItem.BorrowedCopies = int.Parse(words[i + 7]);
                _items.Add(newItem);

            }
            return AuthenticationResult.Ok;
        }
#endif
#if OfflineMode
        public async Task<AuthenticationResult> LoadData()
        {
            for (int i = 0; i < 50; i++)
            {
                var newItem = new Book("Harry Potter", Guid.NewGuid(), Book.BookCategory.Fiction, "Fantasy");
                if (i % 5 == 0) newItem.CoverImage = "http://bookriotcom.c.presscdn.com/wp-content/uploads/2014/08/HP_hc_old_2.jpg";
                if (i % 4 == 0) newItem.CoverImage = "http://mediaroom.scholastic.com/files/HP4cover.jpg";
                _items.Add(newItem);
            }

            for (int i = 0; i < 20; i++)
            {
                var newItem = new Journal("Blaizer", Guid.NewGuid(), Journal.JournalCategory.Men, "");
                if (i % 5 == 0) newItem.CoverImage = "http://orig05.deviantart.net/0c8f/f/2011/024/f/9/happy_man_magazine_by_johnnyrocker666-d37z81t.png";
                if (i % 4 == 0) newItem.CoverImage = "https://israelibreakfast.files.wordpress.com/2012/11/155933_10151241566587342_296280234_n.jpg";
                _items.Add(newItem);
            }
            return AuthenticationResult.Ok;
        }
#endif
    }
}
