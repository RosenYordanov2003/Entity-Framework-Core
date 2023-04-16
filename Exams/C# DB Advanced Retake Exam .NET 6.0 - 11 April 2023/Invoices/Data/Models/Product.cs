namespace Invoices.Data.Models
{
    using Invoices.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        public Product()
        {
            ProductsClients = new HashSet<ProductClient>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public CategoryType CategoryType { get; set; }

        public ICollection<ProductClient> ProductsClients { get; set; } = null!;
    }
}
