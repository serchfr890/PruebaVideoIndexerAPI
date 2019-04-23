﻿using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.IO;
using System.Dynamic;
using Newtonsoft.Json;
using System.Threading.Tasks; 

namespace PruebadeAPIVideoIndexer
{
    class Program
    {
        private static string OCP_APIM_SUBCRIPTION_KEY = "Ocp-Apim-Subscription-Key";
        private static string SUBSCRIPTION_KEY = "9910bef53a484b1ba9eb9961f8815d64";
        private static string LOCATION = "trial";
        private static string ACCOUNTID = "bc129b9e-ee67-4686-ba40-76f051a5911f";

        static void Main(string[] args)
        {
            //string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJBY2NvdW50SWQiOiJiYzEyOWI5ZS1lZTY3LTQ2ODYtYmE0MC03NmYwNTFhNTkxMWYiLCJBbGxvd0VkaXQiOiJGYWxzZSIsIkV4dGVybmFsVXNlcklkIjoiNTgyYWZkNjdiNjhiYWQzMCIsIlVzZXJUeXBlIjoiTWljcm9zb2Z0IiwiaXNzIjoiaHR0cHM6Ly93d3cudmlkZW9pbmRleGVyLmFpLyIsImF1ZCI6Imh0dHBzOi8vd3d3LnZpZGVvaW5kZXhlci5haS8iLCJleHAiOjE1NTU5NzE3NDksIm5iZiI6MTU1NTk2Nzg0OX0.0PRBjM5gxwb_iibMPApsKo8vG_uZEyLDoTA0w-hJLFM";
            var result = Task.Run(() => GetAccountAccesTokenAsync());
            result.Wait();
            string accessToken = result.Result.Trim('\"');
            SearchVideos(accessToken);
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadKey();
        }
        static async Task<string> GetAccountAccesTokenAsync()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add(OCP_APIM_SUBCRIPTION_KEY, SUBSCRIPTION_KEY);
            var uri = "https://api.videoindexer.ai/auth/" + LOCATION + "/Accounts/" + ACCOUNTID + "/AccessToken";// + queryString;
            var responseMessage = await client.GetAsync(uri);
            return await responseMessage.Content.ReadAsStringAsync();
        }

        private static async void SearchVideos(string accessToken)
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

                Console.WriteLine("Se encontró '{0}' en los siguientes videos: ", text);
                foreach (var result45 in jsonResult.results)
                {
                    Console.WriteLine("Nombre: " + result45.name);
                    Console.WriteLine("Tiempo: " + result45.searchMatches[0].startTime);
                    Console.WriteLine("URL: " + "https://www.videoindexer.ai/accounts/bc129b9e-ee67-4686-ba40-76f051a5911f/videos/" + result45.id + "/");
                }
            }
        }

    }
}
