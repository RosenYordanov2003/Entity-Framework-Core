namespace Artillery.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Country")]
    public class ExportCountryDto
    {
        [XmlAttribute]
        public string Country { get; set; } = null!;

        [XmlAttribute]
        public int ArmySize { get; set; }
    }
}
