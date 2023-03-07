namespace CarDealer
{
    using CarDealer.Data;
    using CarDealer.DTOs.Import;
    using CarDealer.Models;
    using System.Xml.Serialization;

    public class StartUp
    {
        private const string MainPath = "../../../Datasets/";
        public static void Main()
        {
            CarDealerContext carDealerContext = new CarDealerContext();
            string inputXml = File.ReadAllText($"{MainPath}sales.xml");
            Console.WriteLine(ImportSales(carDealerContext, inputXml));
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
                    TravelledDistance = car.TraveledDistance
                };
                ICollection<PartCar> partCars = new List<PartCar>();
                foreach (int partId in car.Parts.Select(p=>p.Id).Distinct())
                {
                    if (context.Parts.Any(p=>p.Id==partId))
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

            CustomerDto[]customers = (CustomerDto[])serializer.Deserialize(reader);

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
            XmlSerializer serializer = new XmlSerializer(typeof(SaleDto[]),root);

            using StringReader reader = new StringReader(inputXml);

            SaleDto[] sales = (SaleDto[])serializer.Deserialize(reader);

            ICollection<Sale> salesToAdd = new List<Sale>();

            foreach (SaleDto sale in sales)
            {
                if (!context.Cars.Any(c=>c.Id==sale.CarId))
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
    }
}