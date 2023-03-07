namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;
    public class ExportUsersAndProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }  

        [XmlArray("users")]
        public ExportUserDto[] Users { get; set; }
    }
}
