namespace Trucks.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Trucks.Commnon;

    public class ImportClientDto
    {
        [JsonProperty("Name")]
        [MinLength(CheckValidator.ClientMinNameLength)]
        [MaxLength(CheckValidator.ClientMaxNameLength)]
        [Required]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [Required]
        [MinLength(CheckValidator.NationalityMinLength)]
        [MaxLength(CheckValidator.NationalityMaxLnegth)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        [JsonProperty("Trucks")]
        public int[] Trucks { get; set; }
    }
}
