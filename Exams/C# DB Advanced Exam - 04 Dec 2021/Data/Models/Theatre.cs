namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
   

    public class Theatre
    {
        public Theatre()
        {
            Tickets = new HashSet<Ticket>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public sbyte NumberOfHalls { get; set; }
        [Required]
        public string Director { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
