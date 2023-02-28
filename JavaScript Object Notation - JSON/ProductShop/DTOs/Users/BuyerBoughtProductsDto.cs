using Newtonsoft.Json;

namespace ProductShop.DTOs.Users
{
    [JsonObject]
    public class BuyerBoughtProductsDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("buterFirstName")]
        public string BuyerFirstName { get; set; }

        [JsonProperty("buterLastName")]
        public string BuyerLastName { get; set; }
    }
}
