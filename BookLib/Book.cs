using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Book : AbstractItem
    {
        public Book(string bookName, Guid guid, BookCategory category, string subCategory) : base(bookName, guid)
        {
            _category = category;
            _subCategory = subCategory;
        }

        static Book()
        {
            SubCategoryDict = new Dictionary<BookCategory, List<string>>();
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
            None,
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

        public override string SubCategory
        {
            get { return _subCategory; }
            set
            {
                UpdateDictionary(_subCategory);
                _subCategory = value;
            }
        }

    }
}
