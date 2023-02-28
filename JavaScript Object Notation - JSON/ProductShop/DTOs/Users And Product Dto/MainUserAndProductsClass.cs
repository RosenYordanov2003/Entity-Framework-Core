namespace ProductShop.DTOs.Users_And_Product_Dto
{
    using System.Text.Json.Serialization;
    public class MainUserAndProductsClass
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("users")]
        public UserAndProductUserDto[] Users { get; set; }
    }
}
