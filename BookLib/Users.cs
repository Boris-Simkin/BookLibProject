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
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            string responseBody = string.Empty;
            try
            {
                response = await httpClient.GetAsync("http://simkin.atwebpages.com/GetUsers.php");
                if (response.StatusCode == HttpStatusCode.OK)
                    responseBody = await response.Content.ReadAsStringAsync();
                else
                    return ResultFromServer.ConnectionFailed;
            }
            catch
            {
                return ResultFromServer.ConnectionFailed;
            }

            string[] words = responseBody.Split('^');
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

        public async Task<ResultFromServer> Authentication(User user)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            values.Add("Password", user.Password);
            var content = new FormUrlEncodedContent(values);
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/Login.php", content);
            }
            catch
            {
                return ResultFromServer.ConnectionFailed;
            }
            if (response.StatusCode != HttpStatusCode.OK)
                return ResultFromServer.ConnectionFailed;

            string responseBody = await response.Content.ReadAsStringAsync();

            if (responseBody != "isuser" && responseBody != "isadmin")
                return ResultFromServer.ParamsIncorrect;

            if (responseBody == "isuser")
            {
                CurrentUser.IsAdmin = false;
            }
            else if (responseBody == "isadmin")
            {
                CurrentUser.IsAdmin = true;
            }

            CurrentUser.Username = user.Username;
            return ResultFromServer.Ok;
        }

        public async Task<ResultFromServer> Registration(User user)
        {
            //string username = user.Username;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", user.Username);
            values.Add("FName", user.FirstName);
            values.Add("LName", user.LastName);
            values.Add("Password", user.Password);
            var content = new FormUrlEncodedContent(values);
            ResultFromServer result = ResultFromServer.ConnectionFailed;
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/SignIn.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "created")
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

    }
}
