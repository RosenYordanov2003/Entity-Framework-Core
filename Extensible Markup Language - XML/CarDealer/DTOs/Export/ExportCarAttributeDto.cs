namespace CarDealer.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("car")]
    public class ExportCarAttributeDto
    {
        [XmlIgnore]//For problem number 17 it's not needed
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        //[XmlIgnore]//For problem number 19 it's not needed
        [XmlArray("parts")]
        public ExportPartDto[] Parts { get; set; }
    }
}
