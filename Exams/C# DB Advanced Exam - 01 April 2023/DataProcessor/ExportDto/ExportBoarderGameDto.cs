namespace Boardgames.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    [XmlType("Boardgame")]
    public class ExportBoarderGameDto
    {
        public string BoardgameName { get; set; }
        public int BoardgameYearPublished { get; set; }
    }
}
