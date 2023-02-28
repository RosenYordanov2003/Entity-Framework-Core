namespace ProductShop
{
    using AutoMapper;
    using ProductShop.DTOs.Categories;
    using ProductShop.DTOs.Categories_and_Products_Dto;
    using ProductShop.DTOs.Products;
    using ProductShop.DTOs.Users;
    using ProductShop.DTOs.Users_And_Product_Dto;
    using ProductShop.Models;
    using System.Linq;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDto, User>();

            this.CreateMap<ProductDto, Product>();

            this.CreateMap<CategoryDto, Category>();

            this.CreateMap<CategoryProductDto, CategoryProduct>();

            this.CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(d => d.SellerFullName, m => m.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            this.CreateMap<Product, BuyerBoughtProductsDto>()
                .ForMember(d => d.BuyerFirstName, m => m.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, m => m.MapFrom(s => s.Buyer.LastName));

            this.CreateMap<User, SoldProductsDto>()
                .ForMember(d=>d.SoldProducts,m=>m.MapFrom(s=>s.ProductsSold.Where(p=>p.BuyerId.HasValue)));

            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(d => d.CategoryName, m => m.MapFrom(s => s.Name))
                .ForMember(d => d.ProductCount, m => m.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, m => m.MapFrom(s => $"{s.CategoryProducts.Average(p => p.Product.Price):F2}"))
                .ForMember(d => d.TotalRevenue, m => m.MapFrom(s => $"{s.CategoryProducts.Sum(p => p.ProductId):F2}"));

            this.CreateMap<User, MainUserAndProductsClass>()
                .ForMember(d => d.Count, m => m.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue).Count()));

            this.CreateMap<User, UserAndProductUserDto>()
                .ForMember(d => d.Count, m => m.MapFrom(s => s.ProductsSold.Count));

            this.CreateMap<Product, UsersAndProductsProductDto>()
                .ForMember(d => d.ProductName, m => m.MapFrom(s => s.Name));
        }
    }
}
