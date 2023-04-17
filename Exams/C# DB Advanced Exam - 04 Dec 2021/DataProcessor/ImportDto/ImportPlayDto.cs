using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Common;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {
        [MinLength(GlobalConstants.PlayTitleMinLength)]
        [MaxLength(GlobalConstants.PlayTitleMaxLength)]
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Duration { get; set; } = null!;

        [XmlElement("Raiting")]
        [Range(GlobalConstants.RatingMinValue,GlobalConstants.RatingMaxValue)]
        public double Rating { get; set; }
        [Required]
        public string Genre { get; set; } = null!;
        [MaxLength(GlobalConstants.PlayDescriptionMaxLength)]
        [Required]
        public string Description { get; set; } = null!;
        [MinLength(GlobalConstants.ScreenwriterMinValue)]
        [MaxLength(GlobalConstants.ScreenwriterMaxValue)]
        [Required]
        public string Screenwriter { get; set; } = null!;
    }
}
