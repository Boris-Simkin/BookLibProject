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
        public string CurrentUserName { get; set; }
        public AuthenticationResult CurrentUserType { get; set; }



        public async Task<AuthenticationResult> Authentication(string username, string password)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", username);
            values.Add("Password", password);
            var content = new FormUrlEncodedContent(values);
            AuthenticationResult result = AuthenticationResult.ConnectionFailed;
            try
            {
                response = await httpClient.PostAsync("http://simkin.atwebpages.com/Login.php", content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody == "true verifying")
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
