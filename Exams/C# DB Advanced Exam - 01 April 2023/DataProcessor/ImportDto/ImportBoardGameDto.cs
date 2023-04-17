namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Boardgame")]
    public class ImportBoardGameDto
    {
        [Required]
        [MinLength(GlobalConstants.BoardGameNameMinLength)]
        [MaxLength(GlobalConstants.BoardGameNameMaxLength)]
        public string Name { get; set; }
        [Range(GlobalConstants.RatingMinValue, GlobalConstants.RatingMaxValue)]
        public double Rating { get; set; }
        [Range(GlobalConstants.YearPublishedMinValue, GlobalConstants.YearPublishedMaxValue)]
        public int YearPublished { get; set; }
        [Range(GlobalConstants.CategoryTypeMinValue, GlobalConstants.CategoryTypeMaxValue)]
        public int CategoryType { get; set; }
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
