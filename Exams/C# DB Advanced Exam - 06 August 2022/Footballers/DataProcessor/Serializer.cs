namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCoachDto[] result = context.Coaches
                .Include(c => c.Footballers)
                .ToArray()
                .Where(c => c.Footballers.Any())
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers.Select(f => new ExportFootballerDto()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f => f.Name)
                    .ToArray()
                })
                 .OrderByDescending(c => c.FootballersCount)
                 .ThenBy(c => c.CoachName)
                 .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Coaches");

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCoachDto[]), root);

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

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var result = context
                  .Teams
                  .Where(t => t.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                  .ToArray()
                  .Select(t => new
                  {
                      Name = t.Name,
                      Footballers = t.TeamsFootballers
                      .Where(tf => tf.Footballer.ContractStartDate >= date)
                      .ToArray()
                      .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                      .ThenBy(tf => tf.Footballer.Name)
                      .Select(tf => new
                      {
                          FootballerName = tf.Footballer.Name,
                          ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                          ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                          BestSkillType = tf.Footballer.BestSkillType.ToString(),
                          PositionType = tf.Footballer.PositionType.ToString()
                      })
                      .ToArray()
                  }).OrderByDescending(t => t.Footballers.Length)
                    .ThenBy(t => t.Name)
                    .Take(5)
                    .ToArray();


            string jsonFile = JsonConvert.SerializeObject(result, Formatting.Indented);
            return jsonFile;
        }
    }
}
