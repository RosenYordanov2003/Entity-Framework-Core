namespace Artillery.DataProcessor.ImportDto
{
    using Newtonsoft.Json;

    public class ImportGunCountriesDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
    }
}
