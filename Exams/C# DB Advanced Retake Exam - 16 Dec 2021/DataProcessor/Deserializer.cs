namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Countries");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCountryDto[]), root);

            using StringReader reader = new StringReader(xmlString);

            ImportCountryDto[] dtos = (ImportCountryDto[])serializer.Deserialize(reader);

            ICollection<Country> validCountries = new List<Country>();

            foreach (ImportCountryDto cuntryDto in dtos)
            {
                if (!IsValid(cuntryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Country currentCountry = new Country()
                {
                    CountryName = cuntryDto.CountryName,
                    ArmySize = cuntryDto.ArmySize
                };
                validCountries.Add(currentCountry);
                sb.AppendLine(string.Format(SuccessfulImportCountry, currentCountry.CountryName, currentCountry.ArmySize));
            }
            context.Countries.AddRange(validCountries);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Manufacturers");


            XmlSerializer serializer = new XmlSerializer(typeof(ImportManufactureDto[]), root);

            XmlSerializerNamespaces xmlNamespace = new XmlSerializerNamespaces();
            xmlNamespace.Add("", "");

            using StringReader reader = new StringReader(xmlString);

            ImportManufactureDto[] dtos = (ImportManufactureDto[])serializer.Deserialize(reader);

            ICollection<Manufacturer> validManufacturers = new List<Manufacturer>();

            foreach (ImportManufactureDto manufactureDto in dtos)
            {
                if (!IsValid(manufactureDto) || validManufacturers.Any(m => m.ManufacturerName == manufactureDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Manufacturer currentManufacturer = new Manufacturer()
                {
                    ManufacturerName = manufactureDto.ManufacturerName,
                    Founded = manufactureDto.Founded
                };
                string[] foundInfo = manufactureDto.Founded.Split(", ");
                string foundInfoResult = $"{foundInfo[foundInfo.Length - 2]}, {foundInfo[foundInfo.Length - 1]}";
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, currentManufacturer.ManufacturerName, foundInfoResult));
                validManufacturers.Add(currentManufacturer);
            }
            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Shells");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportShellDto[]), root);

            using StringReader reader = new StringReader(xmlString);

            ImportShellDto[] dtos = (ImportShellDto[])serializer.Deserialize(reader);

            ICollection<Shell> validShells = new List<Shell>();

            foreach (ImportShellDto shellDto in dtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Shell shell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber,
                };
                validShells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }
            context.Shells.AddRange(validShells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportGunDto[] dtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            string[] validGunTypes = { "Howitzer", "Mortar", "FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun" };

            ICollection<Gun> validGuns = new List<Gun>();

            foreach (ImportGunDto gunDto in dtos)
            {
                if(!IsValid(gunDto) || !validGunTypes.Contains(gunDto.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Gun gun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gunDto.GunType),
                    ShellId = gunDto.ShellId,
                };
                foreach (ImportGunCountriesDto gunCountry in gunDto.Countries)
                {
                    CountryGun countryGun = new CountryGun()
                    {
                        CountryId = gunCountry.Id,
                        Gun = gun,
                    };
                    gun.CountriesGuns.Add(countryGun);
                }
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
                validGuns.Add(gun);
            }
            context.Guns.AddRange(validGuns);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}