namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Commnon;

    [XmlType("Truck")]
    public class ImportTruckDto
    {
        [XmlElement("RegistrationNumber")]
        [RegularExpression(CheckValidator.TruckNumberRegex)]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [XmlElement("VinNumber")]
        [MinLength(CheckValidator.MinVinNumberLength)]
        [MaxLength(CheckValidator.MaxVinNumberLength)]
        public string VunNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        [Range(CheckValidator.TankMinCapacity,CheckValidator.TankMaxCapacity)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(CheckValidator.MinCargoCapacity, CheckValidator.MaxCargoCapacity)]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Range(CheckValidator.MinCargoValue, CheckValidator.MaxCargoValue)]
        public int CargoType { get; set; }

        [XmlElement("MakeType")]
        [Range(CheckValidator.MinMakeTypeValue, CheckValidator.MaxMaveTypeValue)]
        public int MakeType { get; set; }
    }
}
