namespace ProductShop.DTOs.Users_And_Product_Dto
{
    using Newtonsoft.Json;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    [JsonObject]
    public class UserAndProductUserDto
    {
        [JsonPropertyName("firstName")]

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("age")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public int? Age { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("products")]
        public UsersAndProductsProductDto[] SoldProducts { get; set; }
    }
}
