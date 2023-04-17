namespace Theatre.DataProcessor.ImportDto
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Theatre.Common;

    public class ImportTheatreDto
    {
        [MinLength(GlobalConstants.TheatreNameMingLengh)]
        [MaxLength(GlobalConstants.TheatreNameMaxgLengh)]
        [Required]
        public string Name { get; set; } = null!;
        [Range(GlobalConstants.NumberOfHallsMinValue,GlobalConstants.NumberOfHallsMaxValue)]
        public sbyte NumberOfHalls { get; set; }
        [MinLength(GlobalConstants.DirectorMinLength)]
        [MaxLength(GlobalConstants.DirectorMaxLength)]
        [Required]
        public string Director { get; set; } = null!;
        [JsonProperty("Tickets")]
        public ImportTicketDto[] Tickets { get; set; }
    }
}
