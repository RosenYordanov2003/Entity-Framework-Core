namespace CarDealer.DTO.Parts
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExpotPartDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonIgnore]
        public decimal Price { get; set; }

        //Because of judge system :(

        [JsonProperty("Price")]
        public string FormattedPrice => string.Format($"{Price:F2}");
    }
}
