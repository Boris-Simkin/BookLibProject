using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    abstract class AbstractItem
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

        protected int _itemID;
    }
}
