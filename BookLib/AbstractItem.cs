using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class AbstractItem
    {
        public AbstractItem(string itemName)
        {
            ItemName = itemName;
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        private DateTime _date;

        public DateTime Date
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

        protected Guid _itemID;

        //private Guid _guid;

        //public Guid Guid
        //{
        //    get { return _guid; }
        //    set { _guid = value; }
        //}

    }
}
