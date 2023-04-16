namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Client")]
    public class ImportClientDto
    {
        [MinLength(GlobalConstants.ClientNameMinLength)]
        [MaxLength(GlobalConstants.ClientNameMaxLength)]
        public string Name { get; set; } = null!;
        [MinLength(GlobalConstants.NumberVatMinLength)]
        [MaxLength(GlobalConstants.NumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;
        [XmlArray("Addresses")]
        public ImportAddressesDto[] Addresses { get; set; }
    }
}
