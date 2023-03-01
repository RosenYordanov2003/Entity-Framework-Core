namespace CarDealer.DTO.Customers
{
    using Newtonsoft.Json;
    using System;

    [JsonObject]
    public class CustomerDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
