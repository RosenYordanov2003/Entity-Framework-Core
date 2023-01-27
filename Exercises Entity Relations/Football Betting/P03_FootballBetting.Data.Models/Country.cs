namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public Country()
        {
            Towns = new HashSet<Town>();
        }

        [Key]
        public int CountryId { get; set; }

        [MaxLength(GlobalConstants.MaxCountryNameLength), Required]
        public string Name { get; set; }

        public virtual ICollection<Town> Towns { get; set; }
    }
}
