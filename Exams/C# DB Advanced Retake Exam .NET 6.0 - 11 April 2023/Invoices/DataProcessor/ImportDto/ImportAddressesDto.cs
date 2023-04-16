namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    [XmlType("Address")]
    public class ImportAddressesDto
    {
        [MinLength(GlobalConstants.StreetNameMinLength)]
        [MaxLength(GlobalConstants.StreetNameMaxLength)]
        public string StreetName { get; set; } = null!;
        public int StreetNumber { get; set; }
        [Required]
        public string PostCode { get; set; } = null!;
        [MinLength(GlobalConstants.CityMinLength)]
        [MaxLength(GlobalConstants.CityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.CountryMinLength)]
        [MaxLength(GlobalConstants.CountryMaxLength)]
        public string Country { get; set; } = null!;
    }
}
