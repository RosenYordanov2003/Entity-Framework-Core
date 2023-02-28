using Newtonsoft.Json;

namespace ProductShop.DTOs.Users
{
    [JsonObject]
    public class SoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lasttName")]
        public string LastName { get; set; }

        [JsonProperty("soldProducts")]
        public BuyerBoughtProductsDto[] SoldProducts { get; set; }

    }
}
