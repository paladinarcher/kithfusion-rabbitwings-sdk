using Newtonsoft.Json;

namespace RabbitWings.Orders
{
    /// <summary>
    /// Represents a request for a transaction.
    /// </summary>
    public class TransactionRequest
    {
        [JsonProperty("user")]
        public UserDetails User { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class UserDetails
    {
        [JsonProperty("myObject")]
        public MyObject MyObject { get; set; }
    }

    public class MyObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
