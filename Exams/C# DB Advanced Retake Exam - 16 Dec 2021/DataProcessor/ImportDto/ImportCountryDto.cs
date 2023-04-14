namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ImportCountryDto
    {
        [Required]
        [MinLength(GlobalConstants.CountryMinNameLength)]
        [MaxLength(GlobalConstants.CountryMaxNameLength)]
        public string CountryName { get; set; } = null!;

        [Range(GlobalConstants.MinArmySize, GlobalConstants.MaxArmySize)]
        public int ArmySize { get; set; }
    }
}
