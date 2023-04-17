namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [MinLength(GlobalConstants.CreatorFirstNameMinLength)]
        [MaxLength(GlobalConstants.CreatorFirstNameMaxLength)]
        [Required]
        public string FirstName { get; set; } = null!;
        [MinLength(GlobalConstants.CreatorLastNameMinLength)]
        [MaxLength(GlobalConstants.CreatorLastNameMaxLength)]
        [Required]
        public string LastName { get; set; } = null!;
        [XmlArray("Boardgames")]
        public ImportBoardGameDto[] Games { get; set; }
    }
}
