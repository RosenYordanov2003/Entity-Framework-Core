namespace CarDealer
{
    using CarDealer.Data;
    using CarDealer.DTOs.Export;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class StartUp
    {
        private const string MainPath = "../../../Datasets/";
        public static void Main()
        {
            CarDealerContext carDealerContext = new CarDealerContext();
            string inputXml = File.ReadAllText($"{MainPath}sales.xml");
            Console.WriteLine(GetTotalSalesByCustomer(carDealerContext));
        }
        //01  Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(SupplierDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            SupplierDto[] suppliers = (SupplierDto[])serializer.Deserialize(reader);

            Supplier[] suppliersToAdd = suppliers
                .Select(s => new Supplier
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter,
                }).ToArray();
            context.Suppliers.AddRange(suppliersToAdd);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        //02 Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Parts");
            XmlSerializer serializer = new XmlSerializer(typeof(PartDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            PartDto[] parts = (PartDto[])serializer.Deserialize(reader);

            ICollection<Part> partsToAdd = new List<Part>();

            foreach (PartDto part in parts)
            {
                if (context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    partsToAdd.Add(new Part()
                    {
                        Name = part.Name,
                        Price = part.Price,
                        Quantity = part.Quantity,
                        SupplierId = part.SupplierId,
                    });
                }
            }
            context.Parts.AddRange(partsToAdd);
            context.SaveChanges();
            return $"Successfully imported {partsToAdd.Count}";
        }
        //03 Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Cars");
            XmlSerializer serializer = new XmlSerializer(typeof(CarDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            CarDto[] cars = (CarDto[])serializer.Deserialize(reader);

            ICollection<Car> carsToAdd = new List<Car>();

            foreach (CarDto car in cars)
            {
                Car carToAdd = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TraveledDistance
                };
                ICollection<PartCar> partCars = new List<PartCar>();
                foreach (int partId in car.Parts.Select(p => p.Id).Distinct())
                {
                    if (context.Parts.Any(p => p.Id == partId))
                    {
                        PartCar partCar = new PartCar()
                        {
                            Car = carToAdd,
                            PartId = partId,
                        };
                        partCars.Add(partCar);
                    }
                }
                carToAdd.PartsCars = partCars;
                carsToAdd.Add(carToAdd);
            }
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();
            return $"Successfully imported {carsToAdd.Count}";
        }
        //04 Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Customers");

            XmlSerializer serializer = new XmlSerializer(typeof(CustomerDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            CustomerDto[] customers = (CustomerDto[])serializer.Deserialize(reader);

            Customer[] customersToAdd = customers
                .Select(c => new Customer
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                }).ToArray();
            context.Customers.AddRange(customersToAdd);
            context.SaveChanges();
            return $"Successfully imported {customersToAdd.Length}";
        }
        //05 Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Sales");
            XmlSerializer serializer = new XmlSerializer(typeof(SaleDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            SaleDto[] sales = (SaleDto[])serializer.Deserialize(reader);

            ICollection<Sale> salesToAdd = new List<Sale>();

            foreach (SaleDto sale in sales)
            {
                if (!context.Cars.Any(c => c.Id == sale.CarId))
                {
                    continue;
                }
                salesToAdd.Add(new Sale()
                {
                    CarId = sale.CarId,
                    CustomerId = sale.CustomerId,
                    Discount = sale.Discount
                });
            }
            context.Sales.AddRange(salesToAdd);
            context.SaveChanges();
            return $"Successfully imported {salesToAdd.Count}";
        }
        //06 Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCarDto[] carDtos = context
                .Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                })
                .ToArray();
            sb.Append(Serialize(carDtos, "cars"));
            return sb.ToString().TrimEnd();
        }
        //07 Export Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCarAttributeDto[] cars = context.Cars
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new ExportCarAttributeDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                }).ToArray();

            sb.Append(Serialize(cars, "cars"));

            return sb.ToString().TrimEnd();
        }
        //08 Export Suppliers

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportSupplierDto[] suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportSupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();

            sb.Append(Serialize(suppliers, "suppliers"));


            return sb.ToString().TrimEnd();
        }
        //09 Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCarAttributeDto[] cars = context.Cars
                .Select(c => new ExportCarAttributeDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars.Select(p => new ExportPartDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price,
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            sb.Append(Serialize(cars, "cars"));

            return sb.ToString().TrimEnd();
        }
        //10 Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCustomerDto[] customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new ExportCustomerDto
                {
                    Name = c.Name,
                    CountCars = c.Sales.Count,
                    IsYoungDriver = c.IsYoungDriver,
                    SpentMoney = c.IsYoungDriver ? c.Sales.SelectMany(c => c.Car.PartsCars).Sum(p => p.Part.Price)
                    : c.Sales.SelectMany(c => c.Car.PartsCars).Sum(p => p.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            sb.Append(Serialize(customers, "customers"));

            return sb.ToString().TrimEnd();
        }
        //11 Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportSaleDto[] sales = context.Sales
                .Select(s => new ExportSaleDto
                {
                    Car = new ExportCarAttributeDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(p => p.Part.Price),
                    TotalPrice = s.Car.PartsCars.Sum(p => p.Part.Price) - (s.Car.PartsCars.Sum(p => p.Part.Price) * s.Discount) / 100
                }).ToArray();
            sb.Append(Serialize(sales, "sales"));
            return sb.ToString().TrimEnd();
        }

        private static string Serialize<T>(T dtos, string rootName)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute rootAttribute = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), rootAttribute);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;


            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();

            nameSpaces.Add(string.Empty, string.Empty);

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(writer, dtos, nameSpaces);
            }
            return sb.ToString().TrimEnd();
        }
    }
}