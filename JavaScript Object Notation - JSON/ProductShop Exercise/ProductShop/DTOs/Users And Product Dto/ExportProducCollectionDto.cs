namespace ProductShop.DTOs.Users_And_Product_Dto
{
    using Newtonsoft.Json;
    public class ExportProducCollectionDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
