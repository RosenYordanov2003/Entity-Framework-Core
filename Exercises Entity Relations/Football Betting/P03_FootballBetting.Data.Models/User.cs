namespace P03_FootballBetting.Data.Models
{
    using P03_FootballBetting.Data.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [MaxLength(GlobalConstants.MaxUserNameLength), Required]
        public string Username { get; set; }

        [Required, MaxLength(GlobalConstants.MaxPasswordLength)]
        public string Password { get; set; }

        [Required, MaxLength(GlobalConstants.MaxEmailLength)]
        public string Email { get; set; }

        [Required, MaxLength(GlobalConstants.MaxNameLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
