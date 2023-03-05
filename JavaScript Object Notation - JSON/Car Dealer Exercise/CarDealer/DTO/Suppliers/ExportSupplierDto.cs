namespace CarDealer.DTO.Suppliers
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportSupplierDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PartsCount { get; set; }
    }
}
