namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Manufacturer")]
    public class ImportManufactureDto
    {
        [Required]
        [MinLength(GlobalConstants.ManufacturerNameMinLength)]
        [MaxLength(GlobalConstants.ManufacturerNameMaxLength)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.FoundedMinLength)]
        [MaxLength(GlobalConstants.FoundedMaxLength)]
        public string Founded { get; set; } = null!;
    }
}
