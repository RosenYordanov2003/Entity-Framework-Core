namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Common;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    public class ImportGunDto
    {
        [JsonProperty("ManufacturerId")]
        public int ManufacturerId { get; set; }

        [JsonProperty("GunWeight")]
        [Range(GlobalConstants.MinGunWeight, GlobalConstants.MaxGunWeight)]
        public int GunWeight { get; set; }

        [JsonProperty("BarrelLength")]
        [Range(GlobalConstants.MinBarrelLength, GlobalConstants.MaxBarrelLength)]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [JsonProperty("Range")]
        [Range(GlobalConstants.RangeMinValue, GlobalConstants.RangeMaxValue)]
        public int Range { get; set; }

        [JsonProperty("GunType")]
        public string GunType { get; set; } = null!;
        [JsonProperty("ShellId")]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public ImportGunCountriesDto[] Countries { get; set; } = null!;

    }
}
