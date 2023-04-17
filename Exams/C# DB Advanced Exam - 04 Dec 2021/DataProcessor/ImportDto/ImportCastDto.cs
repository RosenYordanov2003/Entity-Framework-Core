namespace Theatre.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Theatre.Common;

    [XmlType("Cast")]
    public class ImportCastDto
    {
        [MinLength(GlobalConstants.CastFullNameMinLength)]
        [MaxLength(GlobalConstants.CastFullNameMaxLength)]
        [Required]
        public string FullName { get; set; } = null!;
        public bool IsMainCharacter { get; set; }
        [RegularExpression(GlobalConstants.Regex)]
        [Required]
        public string PhoneNumber { get; set; } = null!;
        public int PlayId { get; set; }
    }
}
