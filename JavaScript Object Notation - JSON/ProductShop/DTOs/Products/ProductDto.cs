using ProductShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Products
{
    public class ProductDto
    {
        [MinLength(3),Required] 
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int SellerId { get; set; }

        public int? BuyerId { get; set; }
    }
}
