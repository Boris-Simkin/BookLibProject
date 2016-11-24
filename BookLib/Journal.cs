using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Journal : AbstractItem
    {
        public Journal(string journalName, Guid guid, JournalCategory category, string subCategory) : base(journalName, guid)
        {
            _category = category;
            _subCategory = subCategory;
        }

        public enum JournalCategory
        {
            Magazine,
            Traveling,
            Sport,
            Men,
            Women,
            Business,
            Law,
            Cars,
            Comics,
            Gardening,
            Culinar
        }

        JournalCategory? _category;

        public JournalCategory? Category
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
            return "magazine";
        }
    }
}
