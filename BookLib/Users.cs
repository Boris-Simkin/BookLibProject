using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class Users : IUsers
    {
        public enum AuthenticationResult
        {
            ConnectionFailed,
            LoginIncorrect,
            IsUser,
            IsAdmin
        }  

        public AuthenticationResult Result { get; set; }

        public async Task<AuthenticationResult> Authentication(string username, string password)
        {
            return AuthenticationResult.IsUser;

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            var values = new Dictionary<string, string>();
            values.Add("Username", username);
            values.Add("Password", password);
            var content = new FormUrlEncodedContent(values);

            response = await httpClient.PostAsync("http://simkin.atwebpages.com/Login.php", content);
            //throw new NotImplementedException();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody != "false verifying")
                {
                    return AuthenticationResult.IsUser;
                }

                else return AuthenticationResult.LoginIncorrect;
            }

            return AuthenticationResult.ConnectionFailed;
        }
    }
}
