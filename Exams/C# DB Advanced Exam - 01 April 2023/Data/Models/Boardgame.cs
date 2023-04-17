namespace Boardgames.Data.Models
{
    using Boardgames.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Boardgame
    {
        public Boardgame()
        {
            BoardgamesSellers = new HashSet<BoardgameSeller>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public double Rating { get; set; }
        public int YearPublished { get; set; }
        public CategoryType CategoryType { get; set; }
        [Required]
        public string Mechanics { get; set; } = null!;
        [ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        public Creator Creator { get; set; }
        public ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
    }
}
