using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    static class SubCategory<T> where T : AbstractItem
    {
        static Dictionary<Book.BookCategory, List<string>> BookSubCategoryDict;
        static Dictionary<Journal.JournalCategory, List<string>> JournalSubCategoryDict;


        static void AddToDict(Book.BookCategory category)
        {
        }

        static void AddToDict(Journal.JournalCategory category)
        {
        }
    }
}
