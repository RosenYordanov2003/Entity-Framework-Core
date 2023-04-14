namespace Artillery.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    [XmlType("Gun")]
    public class ExportGunDto
    {
        [XmlAttribute]
        public string Manufacturer { get; set; } = null!;

        [XmlAttribute]
        public string GunType { get; set; } = null!;

        [XmlAttribute]
        public int GunWeight { get; set; }

        [XmlAttribute]
        public double BarrelLength { get; set; }

        [XmlAttribute]
        public int Range { get; set; }

        [XmlArray]

        public ExportCountryDto[] Countries { get; set; }
    }
}
