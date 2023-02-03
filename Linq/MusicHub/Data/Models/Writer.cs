namespace MusicHub.Data.Models
{
    using MusicHub.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Writer
    {
        public Writer()
        {
            Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(GlobalConstants.MaxWriterNameLength), Required]
        public string Name { get; set; }

        public string Pseudonym { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
