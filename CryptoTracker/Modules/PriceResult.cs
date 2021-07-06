using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CryptoTracker.Modules
{
    public class PriceResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }
    }
}
