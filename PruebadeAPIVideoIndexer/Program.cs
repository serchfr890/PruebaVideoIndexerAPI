using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.IO;
using System.Dynamic;

namespace PruebadeAPIVideoIndexer
{
    class Program
    {

        private static string OCP_APIM_SUBCRIPTION_KEY = "Ocp-Apim-Subscription-Key";
        private static string SUBSCRIPTION_KEY = "9910bef53a484b1ba9eb9961f8815d64";
        private static string LOCATION = "trial";
        private static string ACCOUNTID = "bc129b9e-ee67-4686-ba40-76f051a5911f";
        private static string VIDEOID = "9699eac128";

        private static string ACCOUNTVIDEOACCESS = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJBY2NvdW50SWQiOiJiYzEyOWI5ZS1lZTY3LTQ2ODYtYmE0MC03NmYwNTFhNTkxMWYiLCJWaWRlb0lkIjoiOTY5OWVhYzEyOCIsIkFsbG93RWRpdCI6IlRydWUiLCJFeHRlcm5hbFVzZXJJZCI6IjU4MmFmZDY3YjY4YmFkMzAiLCJVc2VyVHlwZSI6Ik1pY3Jvc29mdCIsImlzcyI6Imh0dHBzOi8vd3d3LnZpZGVvaW5kZXhlci5haS8iLCJhdWQiOiJodHRwczovL3d3dy52aWRlb2luZGV4ZXIuYWkvIiwiZXhwIjoxNTU1NzQyNDQyLCJuYmYiOjE1NTU3Mzg1NDJ9.5dQqtLj-ctaB3N_U9RD5htiLtpq7e_GXNnbpm5pDS5g";
        static void Main(string[] args)
        {
            GetAccountAccesToken();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadKey();
        }

        static async void GetAccountAccesToken()
        {
            Console.WriteLine("GetAccountAccesToken");
            using (var client = new HttpClient())
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                //Request headers
                client.DefaultRequestHeaders.Add(OCP_APIM_SUBCRIPTION_KEY, SUBSCRIPTION_KEY);
                //Request Parameters
                queryString["allowEdit"] = "false";
                var uri = "https://api.videoindexer.ai/auth/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/" + VIDEOID + "/AccessToken?" + queryString;
                //var uri = "https://api.videoindexer.ai/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/" + VIDEOID + "/Index?" + queryString;
                var responseMessage = await client.GetAsync(uri);
                Console.WriteLine(responseMessage);
            }
                

            
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            
            //queryString["languaje"] = "es-ES";
            //queryString["reTranslate"] = "False";
            //queryString["accessToken"] = ACCOUNTVIDEOACCESS;

           
            
        }

        static async void GetVideoIndex()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["languaje"] = "es-ES";
            queryString["reTraslate"] = "False";
            queryString["accessToken"] = ACCOUNTVIDEOACCESS;
            var uri = "https://api.videoindexer.ai/" + LOCATION + "/Accounts/" + ACCOUNTID + "/Videos/" + VIDEOID + "/Index?" + queryString;
            var response = await client.GetAsync(uri);
            Console.WriteLine(response);
        }
    }
}
