namespace Boardgames.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Creator
    {
        public Creator()
        {
            Boardgames = new HashSet<Boardgame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public ICollection<Boardgame> Boardgames { get; set; } = null!;
    }
}
