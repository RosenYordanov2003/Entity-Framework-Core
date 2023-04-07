namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Commnon;

    [XmlType("Despatcher")]
    public class ImportDespatcherDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(CheckValidator.DespetcherNameMaxLengtth)]
        [MinLength(CheckValidator.DespatcherNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; } = null!;
        [XmlArray("Trucks")]
        public ImportTruckDto[] Trucks { get; set; } = null!;
    }
}
