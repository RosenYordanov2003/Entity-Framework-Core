namespace ProductShop.DTOs.Categories_and_Products_Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class CategoryProductDto
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }
    }
}
