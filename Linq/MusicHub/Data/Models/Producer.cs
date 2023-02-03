namespace MusicHub.Data.Models
{
    using MusicHub.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Producer
    {
        public Producer()
        {
            Albums = new HashSet<Album>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(GlobalConstants.ProducerMaxNameLength),Required]
        public string Name { get; set; }
        public string Pseudonym { get; set; }
        public string PhoneNumber { get;set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
