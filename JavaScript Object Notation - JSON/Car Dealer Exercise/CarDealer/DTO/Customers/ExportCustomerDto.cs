namespace CarDealer.DTO.Customers
{
    using Newtonsoft.Json;
    public class ExportCustomerDto
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("boughtCars")]
        public int BoughtCars { get; set; }


        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}
