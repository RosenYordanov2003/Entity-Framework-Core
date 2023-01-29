namespace P01_StudentSystem.Data.Models
{
    using P01_StudentSystem.Data.Common;
    using P01_StudentSystem.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [MaxLength(GlobalConstants.MaxResourceName),Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
