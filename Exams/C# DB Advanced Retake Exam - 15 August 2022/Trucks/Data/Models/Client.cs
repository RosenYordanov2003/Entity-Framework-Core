﻿namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Trucks.Commnon;

    public class Client
    {
        public Client()
        {
            ClientsTrucks = new HashSet<ClientTruck>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CheckValidator.ClientMaxNameLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(CheckValidator.NationalityMaxLnegth)]
        public string Nationality { get;set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = null!;
    }
}
