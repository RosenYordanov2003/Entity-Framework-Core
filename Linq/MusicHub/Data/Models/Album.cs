namespace MusicHub.Data.Models
{
    using MusicHub.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Album
    {
        public Album()
        {
            Songs = new HashSet<Song>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(GlobalConstants.MaxAlbumNameLength), Required]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal Price => this.Songs.Count > 0 ? Songs.Sum(s => s.Price) : 0m;

        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }

        public virtual Producer Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
