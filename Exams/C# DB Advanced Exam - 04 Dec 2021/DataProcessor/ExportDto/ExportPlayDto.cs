namespace Theatre.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    [XmlType("Play")]
    public class ExportPlayDto
    {
        [XmlAttribute]
        public string Title { get; set; } = null!;
        [XmlAttribute]
        public string Duration { get; set; } = null!;
        [XmlAttribute]
        public string Rating { get; set; }
        [XmlAttribute]
        public string Genre { get; set; } = null!;
        [XmlArray("Actors")]
        public ExportActorDto[] Actors { get; set; }
    }
}
