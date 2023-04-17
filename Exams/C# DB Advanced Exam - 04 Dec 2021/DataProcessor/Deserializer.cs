namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Common;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPlayDto[]), root);

            using StringReader reader = new StringReader(xmlString);

            ImportPlayDto[] dtos = (ImportPlayDto[])serializer.Deserialize(reader);
            ICollection<Play> plays = new List<Play>();

            string[] validGenres = { "Drama", "Comedy", "Romance", "Musical" };


            foreach (ImportPlayDto playDto in dtos)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                TimeSpan duration = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture);
                TimeSpan minTime = new TimeSpan(1, 0, 0);
                if (duration < minTime)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (!validGenres.Contains(playDto.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = playDto.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };
                plays.Add(play);
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute root = new XmlRootAttribute("Casts");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCastDto[]), root);
            using StringReader reader = new StringReader(xmlString);
            ImportCastDto[] dtos = (ImportCastDto[])serializer.Deserialize(reader);

            ICollection<Cast> validCasts = new List<Cast>();
            foreach (ImportCastDto castDto in dtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };
                validCasts.Add(cast);
                string role = cast.IsMainCharacter == true ? "main" : "lesser";
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, role));
            }
            context.Casts.AddRange(validCasts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTheatreDto[] dtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);
            ICollection<Theatre> theatres = new List<Theatre>();

            foreach (ImportTheatreDto theatreDto in dtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Theatre theatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director,

                };
                ICollection<Ticket> tickets = new List<Ticket>();
                foreach (ImportTicketDto ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId
                    };
                    tickets.Add(ticket);
                }
                theatre.Tickets = tickets;
                theatres.Add(theatre);
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }
            context.Theatres.AddRange(theatres);
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
