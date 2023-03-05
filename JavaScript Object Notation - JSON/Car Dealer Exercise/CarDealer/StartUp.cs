namespace CarDealer
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CarDealer.Data;
    using CarDealer.DTO.Cars;
    using CarDealer.DTO.Customers;
    using CarDealer.DTO.Parts;
    using CarDealer.DTO.Sales;
    using CarDealer.DTO.Suppliers;
    using CarDealer.Models;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(CarDealerProfile)));
            CarDealerContext context = new CarDealerContext();
            string json = GetCarsWithTheirListOfParts(context);
            File.WriteAllText("../../../Datasets/cars-and-parts.json", json);

        }

        //01 Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ICollection<Supplier> suppliersToAdd = new List<Supplier>();

            SupplierDto[] suppliers = JsonConvert.DeserializeObject<SupplierDto[]>(inputJson);

            foreach (SupplierDto supplier in suppliers)
            {
                suppliersToAdd.Add(Mapper.Map<Supplier>(supplier));
            }
            context.Suppliers.AddRange(suppliersToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {suppliersToAdd.Count}.");
        }

        //02 Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ICollection<Part> partsToAdd = new List<Part>();
            PartDto[] parts = JsonConvert.DeserializeObject<PartDto[]>(inputJson);
            foreach (PartDto part in parts)
            {
                if (part.SupplierId > 31)
                {
                    continue;
                }
                partsToAdd.Add(Mapper.Map<Part>(part));
            }
            context.Parts.AddRange(partsToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {partsToAdd.Count}.");
        }
        //03 Import Cars TODO
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            CarDto[] cars = JsonConvert.DeserializeObject<CarDto[]>(inputJson);
            ICollection<Car> carsToAdd = new List<Car>();
            foreach (CarDto car in cars)
            {
                Car currentCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };
                foreach (int partId in car.PartCars.Distinct())
                {
                    List<int> validIds = context.Parts.Select(p => p.Id).ToList();
                    if (validIds.Contains(partId))
                    {
                        currentCar.PartCars.Add(new PartCar { PartId = partId });
                    }
                }
                carsToAdd.Add(currentCar);
            }
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {carsToAdd.Count}.");
        }
        //04 Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            CustomerDto[] customers = JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);
            ICollection<Customer> customersToAdd = new List<Customer>();

            foreach (CustomerDto customer in customers)
            {
                customersToAdd.Add(Mapper.Map<Customer>(customer));
            }
            context.Customers.AddRange(customersToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {customersToAdd.Count}.");
        }

        //05 Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            SaleDto[] sales = JsonConvert.DeserializeObject<SaleDto[]>(inputJson);

            ICollection<Sale> salesToAdd = new List<Sale>();

            List<int> customersId = context.Customers.Select(c => c.Id).ToList();
            List<int> carsId = context.Cars.Select(c => c.Id).ToList();
            foreach (SaleDto sale in sales)
            {
                if (customersId.Contains(sale.CustomerId) && carsId.Contains(sale.CarId))
                {
                    salesToAdd.Add(Mapper.Map<Sale>(sale));
                }
            }
            context.Sales.AddRange(salesToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {salesToAdd.Count}.");
        }

        //06 Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy",
            };

            CustomerDto[] customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ProjectTo<CustomerDto>()
                .ToArray();
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented, settings);
            return json;
        }

        //08 Export Cars From Make Toyota

        //09 Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            ExportSupplierDto[] suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportSupplierDto>()
                .ToArray();
            string json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return json;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            ExportCarToyotaDto[] cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ExportCarToyotaDto>()
                .ToArray();
            string json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return json;
        }
        //10 Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
           oArray();

            var result = context.Cars
                .Select(x => new
                {
                    car = new ExportCarDto()
                    {
                        Make = x.Make,
                        Model = x.Model,
                        TravelledDistance = x.TravelledDistance,
                    },
                    parts = x.PartCars.Select(x => new ExpotPartDto
                    {
                        Name = x.Part.Name,
                        Price = x.Part.Price
                    }).ToArray()
                }).ToArray();
          

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
        }
        //11 Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            ExportCustomerDto[] result = context.Customers
                .Where(x => x.Sales.Count > 0)
                .Select(x => new ExportCustomerDto
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.SelectMany(x => x.Car.PartCars).Sum(x => x.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
        }
        //12 Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            ExportSaleDto[] result = context.Sales
                .OrderBy(s => s.Id)
                .Take(10)
                .ProjectTo<ExportSaleDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }
    }
}
