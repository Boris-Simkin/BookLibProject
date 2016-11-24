using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Users
    {
        public Users()
        {
            CurrentUser = new User();
        }

        private List<User> _users = new List<User>();

        public List<User> GetUsers()
        {
            return _users;
        }

        public void ClearList()
        {
            _users.Clear();
        }

        public User CurrentUser { get; set; }

        #region Server management methods
        /// <summary>
        /// Getting from the server the guids of all items of the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> GetUserItemsGuid(User user)
        {
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            var result = await Server.Connect("GetUserItemsGuid.php", values);

          if (result == ResultFromServer.Yes)
            {
                // The retrieved content from the server
                string[] words = Server.ResponseWords;
                // Adding the guids to user's guids list
                for (int i = 0; i < (words.Length - 1); i ++)
                    user.MyItems.Add(new Guid(words[i]));

                return ResultFromServer.Yes;
            }
            return ResultFromServer.ConnectionFailed;
        }

        /// <summary>
        /// Transforms the user into an administrator
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> MakeUserAdmin(User user)
        {
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            return await Server.Connect("MakeMeAdmin.php", values);
        }

        /// <summary>
        /// Removing the user from the server
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> RemoveUserFromServer(User user)
        {
            if (user.IsAdmin)
                throw new ArgumentException("Cannot remove administrator user.");

            var values = new Dictionary<string, string>();
            values.Add("Username",  user.Username);
            return await Server.Connect("RemoveUser.php", values);
        }

        /// <summary>
        /// Getting the users from the server
        /// </summary>
        /// <returns></returns>
        public async Task<ResultFromServer> GetUsersFromServer()
        {
            var result = await Server.Connect("GetUsers.php");
            if (result == ResultFromServer.Yes)
            {
                // The retrieved content from the server
                string[] words = Server.ResponseWords;

                for (int i = 0; i < (words.Length - 4); i += 4)
                {
                    User newUser = new User();
                    newUser.Username = words[0 + i];
                    newUser.FirstName = words[1 + i];
                    newUser.LastName = words[2 + i];
                    if (words[3 + i] == "1")
                        newUser.IsAdmin = true;
                    else
                        newUser.IsAdmin = false;
                    _users.Add(newUser);
                }
                return ResultFromServer.Yes;
            }
            return ResultFromServer.ConnectionFailed;
        }

        /// <summary>
        /// Authentication by username and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> Authentication(User user)
        {
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            values.Add("Password", user.Password);
            //Checking if the username and password is correct
            var result = await Server.Connect("Login.php", values);
            if (result == ResultFromServer.Yes)
            {
                values.Remove("Password");
                //Checking if the user is administrator
                var result2 = await Server.Connect("IsAdmin.php", values);
                if (result2 == ResultFromServer.ConnectionFailed)
                    return result2;

                CurrentUser.IsAdmin = result2 == ResultFromServer.Yes;
                CurrentUser.Username = user.Username;
            }
            return result;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResultFromServer> Registration(User user)
        {
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            values.Add("FName", user.FirstName);
            values.Add("LName", user.LastName);
            values.Add("Password", user.Password);
            //Sending the user data to the server
            return await Server.Connect("SignIn.php", values);
        }
        #endregion
    }
}
