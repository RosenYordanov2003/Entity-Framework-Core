using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlAttribute]
        public int BoardgamesCount { get; set; }
        public string CreatorName { get; set; }
        [XmlArray("Boardgames")]
        public ExportBoarderGameDto[] Games { get; set; }
    }
}
