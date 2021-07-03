using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptoTracker.Modules;

namespace CryptoTracker
{
    static class APIHelper
    {
        private const string BASE_URL = "https://api.nomics.com/v1'";
        private static readonly string requestURL = $"{BASE_URL}/currencies/ticker?key={Credentials.API_KEY}&ids=ETH&interval=1d";

        private static readonly HttpClient client = new HttpClient();

        public static string GetPrices()
        {
            Task<string> response = client.GetStringAsync(requestURL);

            return response.Result;
        }

    }


}
