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

        public enum BookCategory
        {
            Book,
            Fiction,
            Adventure,
            Studying,
            Religion,
            Poetry,
            Romance,
            Novel,
            Horror,
            Biographies,
            Diaries,
            Satire
        }

        BookCategory? _category;

        public BookCategory? Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private string _subCategory;

        public override string SubCategory
        {
            get { return _subCategory; }
            set { _subCategory = value; }
        }

        public override string ToString()
        {
            return "book";
        }
    }
}
