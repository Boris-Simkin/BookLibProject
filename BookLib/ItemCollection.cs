﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class ItemCollection
    {

        public ItemCollection()
        {

        }

        List<AbstractItem> _items;

        public bool IsEmpty()
        {
            return !_items.Any();
        }

        public void AddItem(AbstractItem item)
        {

        }

        public AbstractItem GetItem()
        {
            throw new NotImplementedException();
        }
    }
}