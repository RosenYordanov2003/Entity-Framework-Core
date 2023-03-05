namespace CarDealer.DTO.Customers
{
    using Newtonsoft.Json;
    using System;

    [JsonObject]
    public class CustomerDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("BirthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("IsYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
