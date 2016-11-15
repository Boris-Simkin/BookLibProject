﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IItemDetailsPageAdmin
    {
        void SetContent(AbstractItem item);
        event EventHandler<ItemEventArgs> Save;
        event EventHandler<ItemEventArgs> Delete;
    }
}
