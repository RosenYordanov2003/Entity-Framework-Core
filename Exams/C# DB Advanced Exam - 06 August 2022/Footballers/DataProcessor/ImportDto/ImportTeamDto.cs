namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImportTeamDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(GlobalConstant.TeamNameMinLength)]
        [MaxLength(GlobalConstant.TeamNameMaxLength)]
        [RegularExpression(GlobalConstant.TeamNameRegex)]
        public string Name { get; set; } = null!;

        [JsonProperty("Nationality")]
        [MinLength(GlobalConstant.TeamNationalityMinLength)]
        [MaxLength(GlobalConstant.TeamNationalityMaxLength)]
        [Required]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Trophies")]
        public int Trophies { get; set; }

        [JsonProperty("Footballers")]
        public int[] Footballers { get; set; }
    }
}
