namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Clients");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportClientDto[]), root);

            using StringReader stringReader = new StringReader(xmlString);

            ImportClientDto[] dtos = (ImportClientDto[])serializer.Deserialize(stringReader);

            ICollection<Client> validClients = new List<Client>();

            foreach (ImportClientDto clientDto in dtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                };
                ICollection<Address> validAddresses = new List<Address>();
                foreach (ImportAddressesDto addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address address = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country
                    };
                    validAddresses.Add(address);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
                validClients.Add(client);
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportInvoiceDto[] dtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);
            ICollection<Invoice> invoices = new List<Invoice>();

            int[] validClienIds = context.Clients
                .Select(c => c.Id)
                .ToArray();

            foreach (ImportInvoiceDto invoiceDto in dtos)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                DateTime startDate = DateTime.ParseExact(invoiceDto.IssueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(invoiceDto.DueDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                if (startDate >= endDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!validClienIds.Contains(invoiceDto.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = startDate,
                    DueDate = endDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId
                };
                invoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportProductDto[] dtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);
            ICollection<Product> products = new List<Product>();

            int[] validClientIds = context.Clients
                .Select(c => c.Id)
                .ToArray();

            foreach (ImportProductDto productDto in dtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType
                };
                foreach (int clientId in productDto.Clients.Distinct())
                {
                    if (!validClientIds.Contains(clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    ProductClient productClient = new ProductClient()
                    {
                        ClientId = clientId,
                        Product = product
                    };
                    product.ProductsClients.Add(productClient);
                }
                products.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
            }
            context.Products.AddRange(products);
            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
