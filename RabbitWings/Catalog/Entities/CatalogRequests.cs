using Newtonsoft.Json;

namespace RabbitWings.Catalog.Entities
{
    internal class GetItemRequest
    {

        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("sku")]
        public int sku { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }


    }
}