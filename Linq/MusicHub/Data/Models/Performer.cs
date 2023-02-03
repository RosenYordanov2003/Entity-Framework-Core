namespace MusicHub.Data.Models
{
    using MusicHub.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Performer
    {
        public Performer()
        {
            PerformerSongs = new HashSet<SongPerformer>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(GlobalConstants.PerformerNamesMaxLength),Required]
        public string FirstName { get;set; }

        [MaxLength(GlobalConstants.PerformerNamesMaxLength), Required]
        public string LastName { get; set; }

        public int Age { get; set; }

        public decimal NetWorth { get; set; }

        public virtual ICollection<SongPerformer> PerformerSongs  { get; set; }
    }
}
