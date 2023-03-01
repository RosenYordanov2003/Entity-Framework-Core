using Newtonsoft.Json;

namespace CarDealer.DTO.Suppliers
{
    public class SupplierDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isImporter")]
        public bool IsImporter { get; set; }
    }
}
