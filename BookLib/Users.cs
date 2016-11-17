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
    public enum AuthenticationResult
    {
        ConnectionFailed,
        ParamsIncorrect,
        Ok,
    }

    public class Users : IUsers
    {
        public Users()
        {
            CurrentUser = new User();
        }
        //public string CurrentUserName { get; set; }
        public AuthenticationResult CurrentUserType { get; set; }

        public List<User> _users = new List<User>();

        public List<User> GetUsers()
        {
            return _users;
        }

        public User CurrentUser { get; set; }

        public async Task<AuthenticationResult> GetUsersFromServer()
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
                    return AuthenticationResult.ConnectionFailed;
            }
            catch
            {
                return AuthenticationResult.ConnectionFailed;
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
            return AuthenticationResult.Ok;
        }

        public async Task<AuthenticationResult> Authentication(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", username);
            values.Add("Password", password);
            var content = new FormUrlEncodedContent(values);
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/Login.php", content);
            }
            catch
            {
                return AuthenticationResult.ConnectionFailed;
            }
            if (response.StatusCode != HttpStatusCode.OK)
                return AuthenticationResult.ConnectionFailed;

            string responseBody = await response.Content.ReadAsStringAsync();

            if (responseBody != "isuser" && responseBody != "isadmin")
                return AuthenticationResult.ParamsIncorrect;

            if (responseBody == "isuser")
            {
                CurrentUser.IsAdmin = false;
            }
            else if (responseBody == "isadmin")
            {
                CurrentUser.IsAdmin = true;
            }

            CurrentUser.Username = username;
            return AuthenticationResult.Ok;
        }

        public async Task<AuthenticationResult> Registration(string username, string password, string firstName, string lastName)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", username);
            values.Add("FName", firstName);
            values.Add("LName", lastName);
            values.Add("Password", password);
            var content = new FormUrlEncodedContent(values);
            AuthenticationResult result = AuthenticationResult.ConnectionFailed;
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/SignIn.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "created")
                        result = AuthenticationResult.Ok;
                    else
                        result = AuthenticationResult.ParamsIncorrect;
                }
            }
            catch
            {
                result = AuthenticationResult.ConnectionFailed;
            }
            return result;
        }

    }
}
