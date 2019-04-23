using System;
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
        //private static string accessToken;
        static void Main(string[] args)
        {
            VideoIndexerMethods videoIndexerMethods = new VideoIndexerMethods();
            var result = Task.Run(() => videoIndexerMethods.GetAccountAccesTokenAsync());
            result.Wait();
            var accessToken = result.Result.Trim('\"');
            videoIndexerMethods.SearchVideos(accessToken);
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadKey();
        }
    }
}
