namespace ProductShop.DTOs.Users
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class UserDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        [MinLength(3), Required]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }
}
