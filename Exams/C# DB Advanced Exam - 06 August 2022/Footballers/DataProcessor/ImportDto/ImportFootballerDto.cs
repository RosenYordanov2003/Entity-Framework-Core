namespace Footballers.DataProcessor.ImportDto
{
    using Footballers.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(GlobalConstant.MinFootballerNameLength)]
        [MaxLength(GlobalConstant.MaxFootballerNameLength)]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; } = null!;

        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; } = null!;

        [XmlElement("BestSkillType")]
        [Range(GlobalConstant.MinSkillTypeValue, GlobalConstant.MaxSkillTypeValue)]
        public int BestSkillType { get; set; }

        [XmlElement("PositionType")]
        [Range(GlobalConstant.MinPositionValue, GlobalConstant.MaxPositionValue)]
        public int PositionType { get; set; }
    }
}
