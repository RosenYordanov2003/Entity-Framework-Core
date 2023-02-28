namespace ProductShop.DTOs.Categories
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    [JsonObject]
    public class ExportCategoryDto
    {
        [JsonPropertyName("category")]
        public string CategoryName { get; set; }

        [JsonPropertyName("productsCount")]
        public int ProductCount { get; set; }

        [JsonPropertyName("averagePrice")]
        public string AveragePrice { get; set; }

        [JsonPropertyName("totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
