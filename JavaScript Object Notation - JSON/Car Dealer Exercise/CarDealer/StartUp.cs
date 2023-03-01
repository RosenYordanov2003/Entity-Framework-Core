using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Cars;
using CarDealer.DTO.Customers;
using CarDealer.DTO.Parts;
using CarDealer.DTO.Sales;
using CarDealer.DTO.Suppliers;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(CarDealerProfile)));
            CarDealerContext context = new CarDealerContext();
            string json = File.ReadAllText("../../../Datasets/sales.json");
            Console.WriteLine(ImportSales(context, json));
            //context.Database.EnsureCreated();
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
            PartDto[]parts = JsonConvert.DeserializeObject<PartDto[]>(inputJson);
            foreach (PartDto part in parts)
            {
                if (part.SupplierId>31)
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
                    List<int> validIds = context.Parts.Select(p=>p.Id).ToList();
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
            CustomerDto[]customers = JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);
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
            SaleDto[]sales = JsonConvert.DeserializeObject<SaleDto[]>(inputJson);

            ICollection<Sale> salesToAdd = new List<Sale>();

            List<int> customersId = context.Customers.Select(c => c.Id).ToList();
            List<int>carsId = context.Cars.Select(c=>c.Id).ToList();
            foreach (SaleDto sale in sales)
            {
                if (customersId.Contains(sale.CustomerId)&&carsId.Contains(sale.CarId))
                {
                    salesToAdd.Add(Mapper.Map<Sale>(sale));
                }
            }
            context.Sales.AddRange(salesToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {salesToAdd.Count}.");
        }
    }
}