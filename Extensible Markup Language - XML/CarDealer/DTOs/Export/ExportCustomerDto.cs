namespace CarDealer.DTOs.Export
{
    using System.Xml.Serialization;

    [XmlType("customer")]
    public class ExportCustomerDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int CountCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }

        [XmlIgnore]
        public bool IsYoungDriver { get; set; }

        [XmlIgnore]
        public decimal Discount { get; set; }   
    }
}
