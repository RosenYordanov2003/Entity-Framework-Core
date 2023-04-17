namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var result = context.Theatres
                 .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                 .ToArray()
                 .Select(t => new
                 {
                     Name = t.Name,
                     Halls = t.NumberOfHalls,
                     TotalIncome = t.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Sum(t => t.Price),
                     Tickets = t.Tickets.Select(t => new
                     {
                         Price = t.Price,
                         RowNumber = t.RowNumber
                     })
                      .Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                      .OrderByDescending(t => t.Price)
                     .ToArray()
                 })
                 .OrderByDescending(t => t.Halls)
                 .ThenBy(t => t.Name)
                 .ToArray();
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            StringBuilder sb = new StringBuilder();
            ExportPlayDto[] dtos = context.Plays
                .Where(p => p.Rating <= raiting)
                .ToArray()
                .Select(t => new ExportPlayDto()
                {
                    Title = t.Title,
                    Duration = t.Duration.ToString("c", CultureInfo.InvariantCulture),
                    Rating = t.Rating == 0 ? "Premier" : t.Rating.ToString(),
                    Genre = t.Genre.ToString(),
                    Actors = t.Casts.Where(c => c.IsMainCharacter == true).Select(c => new ExportActorDto()
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{t.Title}'."
                    })
                    .OrderByDescending(c => c.FullName)
                    .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportPlayDto[]), root);

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true
            };
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            using(XmlWriter xmlWriter = XmlWriter.Create(sb,settings))
            {
                serializer.Serialize(xmlWriter, dtos, namespaces);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
