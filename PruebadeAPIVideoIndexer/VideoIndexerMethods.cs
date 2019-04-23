using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PruebadeAPIVideoIndexer
{
    class VideoIndexerMethods
    {
        private static string OCP_APIM_SUBCRIPTION_KEY = "Ocp-Apim-Subscription-Key";
        private static string SUBSCRIPTION_KEY = "9910bef53a484b1ba9eb9961f8815d64";
        private static string LOCATION = "trial";
        private static string ACCOUNTID = "bc129b9e-ee67-4686-ba40-76f051a5911f";
        //private static string accessToken;
        public async Task<string> GetAccountAccesTokenAsync()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add(OCP_APIM_SUBCRIPTION_KEY, SUBSCRIPTION_KEY);
            var uri = "https://api.videoindexer.ai/auth/" + LOCATION + "/Accounts/" + ACCOUNTID + "/AccessToken";// + queryString;
            var responseMessage = await client.GetAsync(uri);
            return await responseMessage.Content.ReadAsStringAsync();
        }

        public async Task SearchVideos(string accessToken)
        {
            using (var client = new HttpClient())
            {
                Console.WriteLine(accessToken);
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                // Request parameters
                //accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJBY2NvdW50SWQiOiJiYzEyOWI5ZS1lZTY3LTQ2ODYtYmE0MC03NmYwNTFhNTkxMWYiLCJBbGxvd0VkaXQiOiJGYWxzZSIsIkV4dGVybmFsVXNlcklkIjoiNTgyYWZkNjdiNjhiYWQzMCIsIlVzZXJUeXBlIjoiTWljcm9zb2Z0IiwiaXNzIjoiaHR0cHM6Ly93d3cudmlkZW9pbmRleGVyLmFpLyIsImF1ZCI6Imh0dHBzOi8vd3d3LnZpZGVvaW5kZXhlci5haS8iLCJleHAiOjE1NTYwNjA0NTUsIm5iZiI6MTU1NjA1NjU1NX0.Xk6c9QLxOqpc9hUfB19psZr9eNYO-pXrGlxiJSWiBzI";    
                string text = "integrales";
                queryString["query"] = text;
                queryString["pageSize"] = "25";
                queryString["skip"] = "0";
                queryString["accessToken"] = accessToken;
                var uri = "https://api.videoindexer.ai/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/Search?" + queryString;
                var responseMessage = await client.GetAsync(uri);
                var result = await responseMessage.Content.ReadAsStringAsync();
                var borrar = JsonConvert.DeserializeObject(result);
                Console.WriteLine(borrar);

                var jsonResult = JsonConvert.DeserializeObject<VideoSearchResult>(result);

                queryString.Clear();
                queryString["format"] = "Jpeg";
                queryString["accessToken"] = accessToken;
                var queryStringT = HttpUtility.ParseQueryString(string.Empty);
                
                Console.WriteLine("Se encontró '{0}' en los siguientes videos: ", text);
                foreach (var item in jsonResult.results)
                {
                    Console.WriteLine("\n\nNombre: " + item.name);
                    Console.WriteLine("Tiempo: " + item.searchMatches[0].startTime);
                    queryStringT["t"] = Convert.ToString(Math.Round(TimeSpan.Parse((String)item.searchMatches[0].startTime).TotalSeconds));
                    Console.WriteLine("URL: " + "https://www.videoindexer.ai/accounts/bc129b9e-ee67-4686-ba40-76f051a5911f/videos/" + item.id + "/?" + queryStringT);
                    Console.WriteLine("Thumbnaill: " + "https://api.videoindexer.ai/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/" + item.id + "/Thumbnails/" + item.thumbnailId + "?" + queryString);
                    Console.WriteLine("Text: " + item.searchMatches[0].text);
                    Console.WriteLine(Task.Run(() => GetVideoSourceFileDownloadUrl(accessToken, item.id)));
                }
            }
        }

        public static async Task<string> GetVideoSourceFileDownloadUrl(string accessToken, string videoId)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            //queryString["accessToken"] = accessToken;
            //queryString["allowEdit"] = "true";
            var uri = $"https://api.videoindexer.ai/{LOCATION}/Accounts/{ACCOUNTID}/Videos/{videoId}/SourceFile/DownloadUrl/";
            var response = await client.GetAsync(uri);
            Console.WriteLine(response.Content.ReadAsStringAsync());
            return "";
        }
    }
}
