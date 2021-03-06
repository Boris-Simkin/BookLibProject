﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibBL
{
    public interface ILoginView
    {
        //void SetMainView(List<AbstractItem> BooksSource, List<AbstractItem> JournalsSource);
        void SetMainView();
        void SetRegistrationView();
        void RequestFinished();
        event EventHandler<UserEventArgs> Submit;
        event EventHandler registerBtnClick;
        string StringFromServer { set; }
    }
}
