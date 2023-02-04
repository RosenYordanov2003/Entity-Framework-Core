namespace MusicHub
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            string result = ExportSongsAboveDuration(context,4);
            Console.WriteLine(result);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();
            var albums = context.Albums.Where(a => a.ProducerId.Value == producerId).Select(a => new
            {
                a.Name,
                AlbumReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                ProducerName = a.Producer.Name,
                AlbumTotalPrice = a.Price,
                AlbumSongs = a.Songs.Select
                (s => new
                {
                    s.Name,
                    s.Price,
                    SongWriter = s.Writer.Name
                }
                ).OrderByDescending(s => s.Name).ThenBy(s => s.SongWriter).ToList()
            }).ToList();
            foreach (var album in albums.OrderByDescending(a => a.AlbumTotalPrice))
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.AlbumReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine("-Songs:");
                int songCounter = 0;
                foreach (var song in album.AlbumSongs)
                {
                    sb.AppendLine($"---#{++songCounter}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:F2}");
                    sb.AppendLine($"---Writer: {song.SongWriter}");
                }
                sb.AppendLine($"-AlbumPrice: {album.AlbumTotalPrice:F2}");
            }
            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();
            var songs = context.Songs.Include(s => s.SongPerformers)
                .ThenInclude(s => s.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .ThenInclude(s => s.Producer).ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    PerformerFullName = s.SongPerformers.Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}").FirstOrDefault(),
                    SongWriter = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    SongDuration = s.Duration.ToString("c")
                }).OrderBy(s=>s.SongName).ThenBy(s=>s.SongWriter).ThenBy(s=>s.PerformerFullName).ToList();
            int songCounter = 0;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{++songCounter}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.SongWriter}");
                sb.AppendLine($"---Performer: {song.PerformerFullName}");
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.SongDuration}");
            }
            return sb.ToString().Trim();
        }
    }
}
