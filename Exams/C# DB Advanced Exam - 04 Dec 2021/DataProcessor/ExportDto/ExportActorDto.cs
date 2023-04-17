namespace Theatre.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    [XmlType("Actor")]
    public class ExportActorDto
    {
        [XmlAttribute]
        public string FullName { get; set; } = null!;
        [XmlAttribute]
        public string MainCharacter { get; set; } = null!;
    }
}
