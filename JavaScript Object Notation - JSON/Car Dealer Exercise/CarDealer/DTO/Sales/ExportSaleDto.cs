namespace CarDealer.DTO.Sales
{
    using CarDealer.DTO.Cars;
    using Newtonsoft.Json;
    using System;

    public class ExportSaleDto
    {
        [JsonProperty("car")]
        public ExportCarDto Car { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonIgnore]
        public decimal Discount { get; set; }

        [JsonIgnore]
        public decimal Price { get; set; }

        [JsonIgnore]
        public decimal PriceWithDiscount => Math.Round((Price - (Price * Discount) / 100), 2);

        // Because of judge system :(

        [JsonProperty("Discount")]
        public string FormattedDiscount => string.Format($"{Discount:F2}");

        [JsonProperty("price")]
        public string FormattedPrice => string.Format($"{Price:F2}");

        [JsonProperty("priceWithDiscount")]
        public string FormattedPriceWithDiscount => string.Format($"{PriceWithDiscount:F2}");

    }
}
