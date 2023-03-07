namespace ProductShop.DTOs.Products
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportProductInRangeDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("seller")]
        public string SellerFullName { get; set; }
    }
}
