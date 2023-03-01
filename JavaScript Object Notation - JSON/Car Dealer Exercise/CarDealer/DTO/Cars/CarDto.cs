namespace CarDealer.DTO.Cars
{
    using Newtonsoft.Json;

    public class CarDto
    {
        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("travelledDistance")]
        public long TravelledDistance { get; set; }

        [JsonProperty("partsId")]
        public int[] PartCars { get; set; }
    }
}
