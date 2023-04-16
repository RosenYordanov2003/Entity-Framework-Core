namespace Invoices.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Invoice")]
    public class ExportinvoicesDto
    {
        public int InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string DueDate { get; set; }
        public string Currency { get; set; }
    }
}
