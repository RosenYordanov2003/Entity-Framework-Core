namespace Invoices.DataProcessor.ImportDto
{
    using Invoices.Common;
    using System.ComponentModel.DataAnnotations;
    public class ImportInvoiceDto
    {
        [Range(GlobalConstants.InvoiceNumberMinValue, GlobalConstants.InvoiceNumberMaxValue)]
        public int Number { get; set; }
        [Required]
        public string IssueDate { get; set; } = null!;
        [Required]
        public string DueDate { get; set; } = null!;
        public decimal Amount { get; set; }
        [Range(GlobalConstants.MinCurrencyValue,GlobalConstants.MaxCurrencyValue)]
        public int CurrencyType { get; set; }
        public int ClientId { get; set; }
    }
}
