using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Categories;
using ProductShop.DTOs.Categories_and_Products_Dto;
using ProductShop.DTOs.Products;
using ProductShop.DTOs.Users;
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
            string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            string result = ImportCategoryProducts(context, inputJson);
            Console.WriteLine(result);
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
            CategoryProductDto[]categoryProducts = JsonConvert.DeserializeObject<CategoryProductDto[]>(inputJson);

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

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}