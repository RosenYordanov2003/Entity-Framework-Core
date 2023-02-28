namespace ProductShop
{
    using AutoMapper;
    using ProductShop.DTOs.Categories;
    using ProductShop.DTOs.Categories_and_Products_Dto;
    using ProductShop.DTOs.Products;
    using ProductShop.DTOs.Users;
    using ProductShop.Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDto, User>();

            this.CreateMap<ProductDto, Product>();

            this.CreateMap<CategoryDto, Category>();

            this.CreateMap<CategoryProductDto, CategoryProduct>();
        }
    }
}
