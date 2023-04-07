namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Data.Models.Enums;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Despatchers");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportDespatcherDto[]), root);

            using StringReader reader = new StringReader(xmlString);

            ImportDespatcherDto[] dtos = (ImportDespatcherDto[])serializer.Deserialize(reader);

            ICollection<Despatcher> validDespatchers = new List<Despatcher>();

            foreach (ImportDespatcherDto dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Truck> validTrucks = new HashSet<Truck>();

                foreach (var truck in dto.Trucks)
                {
                    if (!IsValid(truck) || string.IsNullOrWhiteSpace(truck.RegistrationNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck currentTruck = new Truck()
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber = truck.VunNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CargoType,
                        MakeType = (MakeType)truck.MakeType
                    };
                    validTrucks.Add(currentTruck);
                }

                Despatcher despatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position,
                    Trucks = validTrucks,
                };
                validDespatchers.Add(despatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));
            }
            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {

            StringBuilder sb = new StringBuilder();

            ImportClientDto[] dtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            int[] existingTruckIds = context.Trucks.Select(t => t.Id).ToArray();

            ICollection<Client> validClients = new List<Client>();

            foreach (ImportClientDto clientDto in dtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Client client = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type
                };
                foreach (int truckId in clientDto.Trucks.Distinct())
                {
                    if (!existingTruckIds.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    ClientTruck ct = new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId,
                    };
                    client.ClientsTrucks.Add(ct);
                    validClients.Add(client);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }
            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}