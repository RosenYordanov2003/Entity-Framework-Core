namespace ProductShop.DTOs.Users_And_Product_Dto
{
  using Newtonsoft.Json;
    [JsonObject]
    public class ExportProductsDto
    {
        [JsonProperty("count")]
        public int Count => Products.Length;

        [JsonProperty("products")]
        public ExportProducCollectionDto[] Products { get; set; }
    }
}
