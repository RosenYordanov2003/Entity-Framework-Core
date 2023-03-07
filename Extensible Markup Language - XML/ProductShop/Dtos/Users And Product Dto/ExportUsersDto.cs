namespace ProductShop.DTOs.Users_And_Product_Dto
{
    using Newtonsoft.Json;
    using System.Text.Json;

    [JsonObject]
    public class ExportUsersDto
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]

        public int? Age { get; set; }


        [JsonProperty("soldProducts")]
        public ExportProductsDto SoldProducts { get; set; }
    }
}
