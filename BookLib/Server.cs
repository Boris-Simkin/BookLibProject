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
    public enum ResultFromServer
    {
        ConnectionFailed,
        ParamsIncorrect,
        Ok,
    }

    static class Server
    {
        //My hosting address
        const string ServerUrl = "http://simkin.atwebpages.com/";

        //The data you get from the server
        public static string[] ResponseWords { get; private set; }

        enum ProtocolType
        {
            Get,
            Post
        }

        /// <summary>
        /// Request from the server using Get protocol
        /// </summary>
        /// <param name="phpFileName">file name of specific php script</param>
        /// <returns></returns>
        public static async Task<ResultFromServer> Connect(string phpFileName)
        {
            return await Connect(ProtocolType.Get, phpFileName, new Dictionary<string, string>());
        }

        /// <summary>
        /// Request from the server using Post protocol if we need pass data
        /// </summary>
        /// <param name="phpFileName">file name of specific php script</param>
        /// <param name="values"></param>
        /// <returns>the data you send to the server</returns>
        public static async Task<ResultFromServer> Connect(string phpFileName, Dictionary<string, string> values)
        {
            return await Connect(ProtocolType.Post, phpFileName, values);
        }

        private static async Task<ResultFromServer> Connect(ProtocolType protocolType, string phpFileName, Dictionary<string, string> values)
        {

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            var content = new FormUrlEncodedContent(values);
            string responseBody = string.Empty;
            try
            {
                if (protocolType == ProtocolType.Post)
                    response = await httpClient.PostAsync(ServerUrl + phpFileName, content);
                else
                    response = await httpClient.GetAsync(ServerUrl + phpFileName);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseBody = await response.Content.ReadAsStringAsync();

                    switch (responseBody)
                    {
                        case "ok":
                            return ResultFromServer.Ok;
                        case "incorrect":
                            return ResultFromServer.ParamsIncorrect;
                        default:
                            ResponseWords = responseBody.Split('^');
                            break;
                    }
                    //if (/*protocolType == ProtocolType.Post && */responseBody == "incorrect")
                    //    return ResultFromServer.ParamsIncorrect;
                    //else
                    //    ResponseWords = responseBody.Split('^');
                }
                else
                    return ResultFromServer.ConnectionFailed;
            }
            catch
            {
                return ResultFromServer.ConnectionFailed;
            }

            return ResultFromServer.Ok;
        }

    }
}
