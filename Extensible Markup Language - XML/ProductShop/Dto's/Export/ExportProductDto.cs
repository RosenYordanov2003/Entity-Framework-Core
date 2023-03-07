namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("Product")]
    public class ExportProductDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlIgnore] 
        [XmlElement("buyer")]
        public string BuyerFullName { get; set; } //not needed for problem number 6 and problem number 8

        //[XmlIgnore]
        //public bool BuyerFullNameSpecified => !string.IsNullOrWhiteSpace(BuyerFullName);

    }
}
