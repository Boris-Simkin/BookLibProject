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

        static Journal()
        {
            SubCategoryDict = new Dictionary<JournalCategory, List<string>>();
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

        public enum JournalCategory
        {
            None,
            Traveling,
            Sport,
            Men,
            Women,
        }

        JournalCategory _category;

        public JournalCategory Category
        {
            get { return _category; }
            set
            {
                UpdateDictionary(_subCategory);
                _category = value;
            }
        }

        static Dictionary<JournalCategory, List<string>> SubCategoryDict;

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
