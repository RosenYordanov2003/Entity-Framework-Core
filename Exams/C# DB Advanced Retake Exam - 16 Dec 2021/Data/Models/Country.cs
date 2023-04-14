namespace Artillery.Data.Models
{
    using Artillery.Common;
    using System.ComponentModel.DataAnnotations;
    public class Country
    {
        public Country()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CountryMaxNameLength)]
        public string CountryName { get; set; } = null!;

        public int ArmySize { get; set; }

        public virtual ICollection<CountryGun> CountriesGuns { get; set; } = null!;
    }
}
