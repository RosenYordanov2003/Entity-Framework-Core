namespace ProductShop.DTOs.Categories
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    [JsonObject]
    public class CategoryDto
    {
        [MinLength(3),MaxLength(15)]
        [JsonProperty("name"),Required]
        public string Name { get; set; }
    }
}
