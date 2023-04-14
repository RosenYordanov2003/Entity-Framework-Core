namespace Artillery.Data.Models
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    public class Shell
    {
        public Shell()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        public double ShellWeight { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CaliberMaxTextLength)]
        public string Caliber { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } = null!;
    }
}
