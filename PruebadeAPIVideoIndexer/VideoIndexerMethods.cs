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

        public async void SearchVideos(string accessToken)
        {
            Console.WriteLine(accessToken);
            using (var client = new HttpClient())
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                // Request parameters
                string text = "valor";
                queryString["query"] = text;
                queryString["pageSize"] = "25";
                queryString["skip"] = "0";
                queryString["accessToken"] = accessToken;
                var uri = "https://api.videoindexer.ai/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/Search?" + queryString;
                var responseMessage = await client.GetAsync(uri);
                var result = await responseMessage.Content.ReadAsStringAsync();
                var jsonResult = JsonConvert.DeserializeObject<VideoSearchResult>(result);

                queryString.Clear();
                queryString["format"] = "Jpeg";
                queryString["accessToken"] = accessToken;
                Console.WriteLine("Se encontró '{0}' en los siguientes videos: ", text);
                foreach (var item in jsonResult.results)
                {
                    Console.WriteLine("\n\nNombre: " + item.name);
                    Console.WriteLine("Tiempo: " + item.searchMatches[0].startTime);
                    Console.WriteLine("URL: " + "https://www.videoindexer.ai/accounts/bc129b9e-ee67-4686-ba40-76f051a5911f/videos/" + item.id + "/");
                    Console.WriteLine("Thumbnaill: " + "https://api.videoindexer.ai/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/" + item.id + "/Thumbnails/" + item.thumbnailId + "?" + queryString);
                }
            }
        }
    }
}
