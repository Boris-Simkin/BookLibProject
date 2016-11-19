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


        public async Task<ResultFromServer> MakeUserAdmin(User user)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);

            var content = new FormUrlEncodedContent(values);
            ResultFromServer result = ResultFromServer.ConnectionFailed;
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/MakeMeAdmin.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "ok")
                        result = ResultFromServer.Ok;
                    else
                        result = ResultFromServer.ParamsIncorrect;
                }
            }
            catch
            {
                result = ResultFromServer.ConnectionFailed;
            }
            return result;
        }



        public async Task<ResultFromServer> RemoveUserFromServer(User user)
        {
            if (user.IsAdmin)
                throw new ArgumentException("Cannot remove administrator user.");

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);

            var content = new FormUrlEncodedContent(values);
            ResultFromServer result = ResultFromServer.ConnectionFailed;
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/RemoveUser.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "ok")
                        result = ResultFromServer.Ok;
                    else
                        result = ResultFromServer.ParamsIncorrect;
                }
            }
            catch
            {
                result = ResultFromServer.ConnectionFailed;
            }
            return result;
        }

        public async Task<ResultFromServer> GetUsersFromServer()
        {
            var result = await Server.Connect("GetUsers.php");
            if (result == ResultFromServer.Ok)
            {
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
                return ResultFromServer.Ok;
            }
            return ResultFromServer.ConnectionFailed;
        }

        public async Task<ResultFromServer> Authentication(User user)
        {
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            values.Add("Password", user.Password);
            var result = await Server.Connect("Login.php", values);
            //var content = new FormUrlEncodedContent(values);
            //try
            //{
            //    response = await httpClient.PostAsync("http://simkin.atwebpages.com/Login.php", content);
            //}
            //catch
            //{
            //    return ResultFromServer.ConnectionFailed;
            //}
            //if (response.StatusCode != HttpStatusCode.OK)
            //    return ResultFromServer.ConnectionFailed;

            //string responseBody = await response.Content.ReadAsStringAsync();

            //if (responseBody != "isuser" && responseBody != "isadmin")
            //    return ResultFromServer.ParamsIncorrect;

            //if (responseBody == "isuser")
            //{
            //    CurrentUser.IsAdmin = false;
            //}
            //else if (responseBody == "isadmin")
            //{
            //    CurrentUser.IsAdmin = true;
            //}
            if (result == ResultFromServer.Ok)
            {
                values.Remove("Password");
                var result2 = await Server.Connect("IsAdmin.php", values);
                if (result2 == ResultFromServer.ConnectionFailed)
                    return result2;

                CurrentUser.IsAdmin = Server.ResponseWords[0] == "1";
                CurrentUser.Username = user.Username;
            }

            return ResultFromServer.Ok;
        }

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

    }
}
