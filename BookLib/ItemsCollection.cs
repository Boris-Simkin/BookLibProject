using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<AbstractItem> GetBooks()
        {
            return new List<AbstractItem>(_items.Where(item => item is Book)); 
        }

        public List<AbstractItem> GetJournals()
        {
            return new List<AbstractItem>(_items.Where(item => item is Journal));
        }

        public void LoadData()
        {
            for (int i = 0; i < 50; i++)
            {
                var newItem = new Book("Harry Potter", 234 /*+ i*/, Book.BookCategory.Fiction, "Fantasy");
                if (i % 5 == 0) newItem.CoverImage = "http://bookriotcom.c.presscdn.com/wp-content/uploads/2014/08/HP_hc_old_2.jpg";
                if (i % 4 == 0) newItem.CoverImage = "http://mediaroom.scholastic.com/files/HP4cover.jpg";
                _items.Add(newItem);
            }

            for (int i = 0; i < 20; i++)
            {
                var newItem = new Journal("Blaizer", 234 /*+ i*/, Journal.JournalCategory.Men, "");
                if (i % 5 == 0) newItem.CoverImage = "http://orig05.deviantart.net/0c8f/f/2011/024/f/9/happy_man_magazine_by_johnnyrocker666-d37z81t.png";
                if (i % 4 == 0) newItem.CoverImage = "https://israelibreakfast.files.wordpress.com/2012/11/155933_10151241566587342_296280234_n.jpg";
                _items.Add(newItem);
            }
        }
    }
}
