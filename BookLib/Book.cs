using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Book : AbstractItem
    {
        public Book(string bookName, int isdn, BookCategory category, string subCategory) : base(bookName)
        {
            ISBN = isdn;
            _category = category;
            _subCategory = subCategory;
        }

        void UpdateDictionary(string value)
        {
            //Adding the category key to the dictionary if it's not exist
            if (!SubCategoryDict.ContainsKey(_category))
                SubCategoryDict.Add(_category, new List<string>());
            //Adding the sub category to the dictionary
            SubCategoryDict[_category].Add(value);
            _subCategory = value;
        }

        public enum BookCategory
        {
            Fiction,
            Studying,
            Religion,
            Poetry,
        }

        BookCategory _category;

        public BookCategory Category
        {
            get { return _category; }
            set
            {
                UpdateDictionary(_subCategory);
                _category = value;
            }
        }

        static Dictionary<BookCategory, List<string>> SubCategoryDict;



        private string _subCategory;

        public string SubCategory
        {
            get { return _subCategory; }
            set
            {
                UpdateDictionary(_subCategory);
                _subCategory = value;
            }
        }

        public int ISBN
        {
            get { return _itemID; }
            private set { _itemID = value; }
        }
    }
}
