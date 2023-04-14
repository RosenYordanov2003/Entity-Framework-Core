
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var result = context
                 .Shells.ToArray()
                  .Where(s => s.ShellWeight > shellWeight)
                  .Select(s => new
                  {
                      ShellWeight = s.ShellWeight,
                      Caliber = s.Caliber,
                      Guns = s.Guns.Where(g => g.GunType.ToString() == "AntiAircraftGun").Select(g => new
                      {
                          GunType = g.GunType.ToString(),
                          GunWeight = g.GunWeight,
                          BarrelLength = g.BarrelLength,
                          Range = g.Range > 3000 ? "Long-range" : "Regular range"
                      })
                      .OrderByDescending(g => g.GunWeight)
                      .ToArray()
                  })
                  .OrderBy(s => s.ShellWeight)
                  .ToArray();

           return JsonConvert.SerializeObject(result,Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            StringBuilder sb = new StringBuilder();

            ExportGunDto[] guns = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .ToArray()
                .Select(g => new ExportGunDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    BarrelLength = g.BarrelLength,
                    GunWeight = g.GunWeight,
                    Range = g.Range,
                    Countries = g.CountriesGuns.Select(gc => new ExportCountryDto(){

                        Country = gc.Country.CountryName,
                        ArmySize = gc.Country.ArmySize

                    })
                    .Where(gc => gc.ArmySize > 4500000)
                    .OrderBy(gc => gc.ArmySize)
                    .ToArray()

                })
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Guns");

            XmlSerializer serializer = new XmlSerializer(typeof(ExportGunDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
            };

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(writer, guns, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
