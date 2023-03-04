namespace ProductShop.DTOs.Users_And_Product_Dto
{
    using Newtonsoft.Json;
    public class ExportUserCollectionsDto
    {
        [JsonProperty("usersCount")]
        public int Count =>Users.Length;

        [JsonProperty("users")]
        public ExportUsersDto[] Users { get; set; }
    }
}
