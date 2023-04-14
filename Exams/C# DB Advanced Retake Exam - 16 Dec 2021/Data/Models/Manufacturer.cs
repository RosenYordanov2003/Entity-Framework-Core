namespace Artillery.Data.Models
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new HashSet<Gun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CountryMaxNameLength)]
        public string ManufacturerName { get; set; } = null!;

        [Required]
        [MaxLength(GlobalConstants.FoundedMaxLength)]
        public string Founded { get; set; } = null!;

        public virtual ICollection<Gun> Guns { get; set; } = null!;
    }
}
