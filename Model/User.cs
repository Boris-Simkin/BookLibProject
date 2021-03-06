﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Guid> MyItems { get; set; }

        public User()
        {
            MyItems = new List<Guid>();
        }

        /// <summary>
        /// Checking if the user is reading specific item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsReading(AbstractItem item)
        {
            return MyItems.Contains(item.Guid);
        }

    }
}
