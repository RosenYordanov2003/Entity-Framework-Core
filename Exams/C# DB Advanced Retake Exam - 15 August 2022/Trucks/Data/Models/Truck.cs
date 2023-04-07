namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Trucks.Commnon;
    using Trucks.Data.Models.Enums;
    public class Truck
    {
        public Truck()
        {
            Trucks = new HashSet<Truck>();
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(CheckValidator.MaxRegistrationnumberLength)]
        public string RegistrationNumber { get; set; } = null!;

        [MaxLength(CheckValidator.MaxVinNumberLength)]
        public string VinNumber { get; set; } = null!;
        public int TankCapacity { get; set; }
        public int CargoCapacity { get; set; }

        public CategoryType CategoryType { get; set; }

        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }

        public Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<Truck> Trucks { get; set; } = null!;
    }
}
