namespace Invoices.DataProcessor.ExportDto
{
    using System.Xml.Serialization;
    [XmlType("Client")]
    public class ExportClientDto
    {
        [XmlAttribute]
        public  int InvoicesCount { get; set; }
        public string ClientName { get; set; }
        public string VatNumber { get; set; }
        [XmlArray]
        public ExportinvoicesDto[] Invoices { get; set; }
    }
}
