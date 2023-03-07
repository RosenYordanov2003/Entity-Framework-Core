namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")] //for problem number 8 it's needed
        public int? Age { get; set; }

        [XmlIgnore]// for problem number 8 it's not needed
        [XmlArray("soldProducts")]
        public ExportProductDto[] SoldProducts { get; set; }

        [XmlElement("SoldProducts")]
        public ExportSoldProductsDto Products { get; set; }
    }
}
