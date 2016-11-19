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
                return new List<AbstractItem>(_items.Where(p => p.ItemName.ToLower().Contains(itemName.ToLower()) && p is Book));
            else
                return new List<AbstractItem>(_items.Where(p => p.ItemName.ToLower().Contains(itemName.ToLower()) && p is Journal));

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



        public async Task<ResultFromServer> UpdateInServer(AbstractItem item)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            ResultFromServer result = ResultFromServer.ConnectionFailed;

            var values = new Dictionary<string, string>();
            values.Add("BookName", item.ItemName);
            values.Add("Date", ((DateTimeOffset)item.Date).ToString("d"));
            values.Add("CopyNumber", item.CopyNumber.ToString());
            values.Add("Guid", item.Guid.ToString());
            //send the url encrypted
            values.Add("CoverImage", Crypt.Encrypt("boris", item.CoverImage));
            if (item is Book)
                values.Add("Category", ((Book)item).Category.ToString());
            else
                values.Add("Category", ((Journal)item).Category.ToString());
            values.Add("SubCategory", item.SubCategory);
            values.Add("BorrowedCopies", item.BorrowedCopies.ToString());

            var content = new FormUrlEncodedContent(values);

            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/UpdateBook.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "updated")
                        result = ResultFromServer.Ok;
                }
                else
                    result = ResultFromServer.ConnectionFailed;
            }
            catch
            {
                result = ResultFromServer.ConnectionFailed;
            }
            return result;
        }


        public async Task<ResultFromServer> DeleteFromServer(AbstractItem item)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            ResultFromServer result = ResultFromServer.ConnectionFailed;

            var values = new Dictionary<string, string>();
            values.Add("Guid", item.Guid.ToString());

            var content = new FormUrlEncodedContent(values);

            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/DeleteBook.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "deleted")
                        result = ResultFromServer.Ok;
                }
                else
                    result = ResultFromServer.ConnectionFailed;
            }
            catch
            {
                result = ResultFromServer.ConnectionFailed;
            }
            return result;
        }




        public async Task<ResultFromServer> AddItemToServer(AbstractItem item)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            ResultFromServer result = ResultFromServer.ConnectionFailed;

            var values = new Dictionary<string, string>();
            values.Add("BookName", item.ItemName);
            values.Add("Date", ((DateTimeOffset)item.Date).ToString("d"));
            values.Add("CopyNumber", item.CopyNumber.ToString());
            values.Add("Guid", item.Guid.ToString());
            //send the url encrypted
            values.Add("CoverImage", Crypt.Encrypt("boris", item.CoverImage));
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
                    if (responseBody == "created")
                        result = ResultFromServer.Ok;
                }
                else
                    result = ResultFromServer.ConnectionFailed;
            }
            catch
            {
                result = ResultFromServer.ConnectionFailed;
            }
            return result;
        }



#if !OfflineMode
        public async Task<ResultFromServer> LoadDataFromServer()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            string responseBody = string.Empty;
            try
            {
                response = await httpClient.GetAsync("http://simkin.atwebpages.com/GetBooks.php");
                if (response.StatusCode == HttpStatusCode.OK)
                    responseBody = await response.Content.ReadAsStringAsync();
                else
                    return ResultFromServer.ConnectionFailed;
            }
            catch
            {
                return ResultFromServer.ConnectionFailed;
            }

            string[] words = responseBody.Split('^');
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
            return ResultFromServer.Ok;
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
