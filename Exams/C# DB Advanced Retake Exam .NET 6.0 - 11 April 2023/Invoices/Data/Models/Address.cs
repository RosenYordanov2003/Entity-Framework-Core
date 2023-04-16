namespace Invoices.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StreetName { get; set; } = null!;
        public int StreetNumber { get; set; }
        [Required]
        public string PostCode { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
