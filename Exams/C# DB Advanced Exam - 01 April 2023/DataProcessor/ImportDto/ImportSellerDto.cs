namespace Boardgames.DataProcessor.ImportDto
{
    using Boardgames.Common;
    using System.ComponentModel.DataAnnotations;
    public class ImportSellerDto
    {
        [MinLength(GlobalConstants.SellerNameMinLength)]
        [MaxLength(GlobalConstants.SellerNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;
        [MinLength(GlobalConstants.SellerAddressMinLength)]
        [MaxLength(GlobalConstants.SellerAddressMaxLength)]
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [RegularExpression(GlobalConstants.Regex)]
        [Required]
        public string Website { get; set; } = null!;
        public int[] Boardgames { get; set; }
    }
}
