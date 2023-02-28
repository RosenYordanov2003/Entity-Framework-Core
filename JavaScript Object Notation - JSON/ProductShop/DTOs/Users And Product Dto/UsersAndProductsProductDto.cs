namespace ProductShop.DTOs.Users_And_Product_Dto
{
    using System.Text.Json.Serialization;
    public class UsersAndProductsProductDto
    {
        [JsonPropertyName("name")]
        public string ProductName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
