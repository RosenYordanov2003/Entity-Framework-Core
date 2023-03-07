namespace ProductShop.DTOs.Categories
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportCategoryDto
    {
        [JsonProperty("category")]
        public string CategoryName { get; set; }

        [JsonProperty("productsCount")]
        public int ProductCount { get; set; }

        [JsonProperty("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty("totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
