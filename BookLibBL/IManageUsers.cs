﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibBL
{
    public interface IManageUsers
    {
        event EventHandler<UserEventArgs> MakeAdmin;
        event EventHandler<UserEventArgs> DeleteUser;
        event EventHandler PageLoaded;
        List<User> SourceList { set; }
        void RequestFinished(bool removeFromTable);
    }
}
