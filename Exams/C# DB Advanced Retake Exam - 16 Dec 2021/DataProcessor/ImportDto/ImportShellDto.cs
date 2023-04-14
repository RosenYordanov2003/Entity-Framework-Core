namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Shell")]
    public class ImportShellDto
    {
        [Range(GlobalConstants.MinShellWeight, GlobalConstants.MaxShellWeight)]
        public double ShellWeight { get; set; }

        [Required]
        [MinLength(GlobalConstants.CaliberMinTextLength)]
        [MaxLength(GlobalConstants.CaliberMaxTextLength)]
        public string Caliber { get; set; } = null!;
    }
}
