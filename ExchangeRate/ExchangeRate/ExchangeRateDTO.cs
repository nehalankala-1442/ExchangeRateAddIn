using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExchangeRate
{
    public class ExchangeRateDTO
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
    }
}