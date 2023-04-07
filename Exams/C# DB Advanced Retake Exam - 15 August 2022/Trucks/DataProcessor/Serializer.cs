namespace Trucks.DataProcessor
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Trucks.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportDespatcherDto[] result = context.Despatchers
                .Where(d => d.Trucks.Any())
                .Select(d => new ExportDespatcherDto()
                {
                    TrucksCount = d.Trucks.Count,
                    DespatcherName = d.Name,
                    Trucks = d.Trucks.Select(t => new ExportTruckDto()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType.ToString()
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()

                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Despatchers");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportDespatcherDto[]), root);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();

            nameSpaces.Add(string.Empty, string.Empty);

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(writer, result, nameSpaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                  .Include(c => c.ClientsTrucks)
                  .ThenInclude(ct => ct.Truck)
                  .ToArray()
                  .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                  .Select(c => new
                  {
                      Name = c.Name,
                      Trucks = c.ClientsTrucks.Where(ct => ct.Truck.TankCapacity >= capacity)
                      .Select(t => new
                      {
                          TruckRegistrationNumber = t.Truck.RegistrationNumber,
                          VinNumber = t.Truck.VinNumber,
                          TankCapacity = t.Truck.TankCapacity,
                          CargoCapacity = t.Truck.CargoCapacity,
                          CategoryType = t.Truck.CategoryType.ToString(),
                          MakeType = t.Truck.MakeType.ToString(),
                      }).OrderBy(t => t.MakeType)
                      .ThenByDescending(t => t.CargoCapacity)
                      .ToArray()
                  })
                  .OrderByDescending(c => c.Trucks.Length)
                  .ThenBy(c => c.Name)
                  .Take(10)
                  .ToArray();
            string json = JsonConvert.SerializeObject(clients, Formatting.Indented);

            return json;
        }
    }
}
