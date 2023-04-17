namespace Theatre.Data.Models
{
    using global::Theatre.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Sockets;

    public class Play
    {
        public Play()
        {
            Casts = new HashSet<Cast>();
            Tickets = new HashSet<Ticket>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public TimeSpan Duration { get; set; }
        public double Rating { get; set; }
        public Genre Genre { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Screenwriter { get; set; } = null!;
        public ICollection<Cast> Casts { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
