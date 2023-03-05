﻿namespace CarDealer.DTO.Cars
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ExportCarToyotaDto
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }
    }
}
