using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Categories;
using ProductShop.DTOs.Categories_and_Products_Dto;
using ProductShop.DTOs.Products;
using ProductShop.DTOs.Users;
using ProductShop.DTOs.Users_And_Product_Dto;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));
            ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            string json = GetUsersWithProducts(context);
            File.WriteAllText("../../../Datasets/users-and-products.json", json);
        }
        //02. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ICollection<User> usersToAdd = new List<User>();
            UserDto[] users = JsonConvert.DeserializeObject<UserDto[]>(inputJson);
            foreach (UserDto user in users)
            {
                if (IsValid(user))
                {
                    usersToAdd.Add(Mapper.Map<User>(user));
                }
            }
            context.Users.AddRange(usersToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {users.Length}");
        }
        //03 Import Products

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ProductDto[] products = JsonConvert.DeserializeObject<ProductDto[]>(inputJson);

            ICollection<Product> productsToAdd = new List<Product>();

            foreach (ProductDto product in products)
            {
                if (IsValid(product))
                {
                    productsToAdd.Add(Mapper.Map<Product>(product));
                }
            }
            context.Products.AddRange(productsToAdd);

            context.SaveChanges();

            return string.Format($"Successfully imported {productsToAdd.Count}");
        }
        //04 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            CategoryDto[] categories = JsonConvert.DeserializeObject<CategoryDto[]>(inputJson);

            ICollection<Category> categoriesToAdd = new List<Category>();
            foreach (CategoryDto category in categories)
            {
                if (IsValid(category))
                {
                    categoriesToAdd.Add(Mapper.Map<Category>(category));
                }
            }
            context.Categories.AddRange(categoriesToAdd);
            context.SaveChanges();
            return string.Format($"Successfully imported {categoriesToAdd.Count}");
        }

        //05 Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProductDto[] categoryProducts = JsonConvert.DeserializeObject<CategoryProductDto[]>(inputJson);

            ICollection<CategoryProduct> collectionToAdd = new List<CategoryProduct>();

            foreach (CategoryProductDto categoryProduct in categoryProducts)
            {
                if (IsValid(categoryProduct))
                {
                    collectionToAdd.Add(Mapper.Map<CategoryProduct>(categoryProduct));
                }
            }
            context.CategoryProducts.AddRange(collectionToAdd);
            context.SaveChanges();

            return string.Format($"Successfully imported {collectionToAdd.Count}");
        }

        //06 Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductInRangeDto[] result = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductInRangeDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        //07 Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            UsersSoldProductsDto[] result = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<UsersSoldProductsDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        //08 Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            ExportCategoryDto[] result = context.Categories
                 .OrderByDescending(c => c.CategoryProducts.Count)
                 .ProjectTo<ExportCategoryDto>()
                 .ToArray();
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
        }

        //09 Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            ExportUserCollectionsDto dto = new ExportUserCollectionsDto()
            {
                Users = context.Users.Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderByDescending(u => u.ProductsSold.Where(p => p.BuyerId.HasValue).Count())
                .Select(x => new ExportUsersDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new ExportProductsDto
                    {
                        Products = x.ProductsSold.Where(p => p.BuyerId.HasValue)
                        .Select(x => new ExportProducCollectionDto
                        {
                            Name = x.Name,
                            Price = x.Price,
                        }).ToArray()
                    }
                }).ToArray()
            };
            string json = JsonConvert.SerializeObject(dto, Formatting.Indented, settings);
            return json;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}
