namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    public class ImportProductDto
    {
        [Required]
        [MinLength(GlobalConstants.ProductNameMingength)]
        [MaxLength(GlobalConstants.ProductNameMaxgength)]
        public string Name { get; set; } = null!;
        [Range(typeof(decimal), "5.00", "1000.00")]
        public decimal Price { get; set; }

        [Range(GlobalConstants.CategoryTypeMinValue, GlobalConstants.CategoryTypeMaxValue)]
        public int CategoryType { get; set; }
        public int[] Clients { get; set; }
    }
}
