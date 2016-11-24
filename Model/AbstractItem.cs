using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class AbstractItem
    {
        public AbstractItem(string itemName, Guid guid)
        {
            ItemName = itemName;
            Guid = guid;
            CopyNumber = 1;
            Date = new DateTimeOffset();
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public int BorrowedCopies { get; set; }

        private DateTimeOffset? _date;

        public DateTimeOffset? Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private int _copyNumber;

        public int CopyNumber
        {
            get { return _copyNumber; }
            set { _copyNumber = value; }
        }

        private string _coverImage;

        public string CoverImage
        {
            get { return _coverImage; }
            set { _coverImage = value; }
        }

        public abstract string SubCategory
        {
            get;
            set;
        }
        private Guid _guid;

        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

    }
}
