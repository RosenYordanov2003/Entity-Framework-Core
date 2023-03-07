namespace ProductShop
{
    using System;
    using ProductShop.Data;
    using ProductShop.Dtos.Import;
    using ProductShop.Models;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using ProductShop.Dtos.Export;
    using System.Text;

    public class StartUp
    {
        private const string MainPath = "../../../Datasets/";

        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            string inputXmlFile = File.ReadAllText($"{MainPath}categories-products.xml");
            Console.WriteLine(GetUsersWithProducts(context));
        }

        //01 Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]), root);
            using StringReader stringReader = new StringReader(inputXml);
            ImportUserDto[] users = (ImportUserDto[])serializer.Deserialize(stringReader);

            User[] usersToAdd = users
                .Select(u => new User
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age
                }).ToArray();

            context.Users.AddRange(usersToAdd);
            context.SaveChanges();
            return $"Successfully imported {usersToAdd.Length}";
        }
        //02 Import Products

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductsDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            ImportProductsDto[] products = (ImportProductsDto[])serializer.Deserialize(reader);

            Product[] productsToAdd = products
                .Select(p => new Product
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerId = p.SellerId,
                    BuyerId = p.BuyerId
                }).ToArray();

            context.Products.AddRange(productsToAdd);
            context.SaveChanges();
            return $"Successfully imported {productsToAdd.Length}";
        }

        //03 Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Categories");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryDto[]), root);

            using StringReader reader = new StringReader(inputXml);

            ImportCategoryDto[] categories = (ImportCategoryDto[])serializer.Deserialize(reader);

            Category[] categoriesToAdd = categories
                .Select(c => new Category
                {
                    Name = c.Name,
                }).ToArray();

            context.Categories.AddRange(categoriesToAdd);

            context.SaveChanges();

            return $"Successfully imported {categoriesToAdd.Length}";
        }
        //04 Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("CategoryProducts");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]), root);

            StringReader reader = new StringReader(inputXml);

            ImportCategoryProductDto[] productsCategories = (ImportCategoryProductDto[])serializer.Deserialize(reader);

            List<CategoryProduct> categoryProductsToAdd = new List<CategoryProduct>();

            foreach (ImportCategoryProductDto item in productsCategories)
            {
                if (context.Products.Any(p => p.Id == item.ProductId) && context.Categories.Any(c => c.Id == item.CategoryId))
                {
                    categoryProductsToAdd.Add(new CategoryProduct()
                    {
                        CategoryId = item.CategoryId,
                        ProductId = item.ProductId
                    });
                }
            }

            context.CategoryProducts.AddRange(categoryProductsToAdd);
            context.SaveChanges();
            return $"Successfully imported {categoryProductsToAdd.Count}";
        }
        // 05 Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();


            ExportProductDto[] exportProducts = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ExportProductDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    BuyerFullName = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
                })
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportProductDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, exportProducts, namespaces);

            return sb.ToString().TrimEnd();
        }
        //06 Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportUserDto[] exportUsers = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new ExportUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ExportProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                }).ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportUserDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, exportUsers, namespaces);

            return sb.ToString().TrimEnd();
        }
        //07 Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCategoryDto[] exportCategories = context.Categories
                .Select(c => new ExportCategoryDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Count > 0 ? c.CategoryProducts.Average(c => c.Product.Price) : 0,
                    TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
                })
                .OrderByDescending(p => p.Count)
                .ThenBy(p => p.TotalRevenue)
                .ToArray();
            XmlRootAttribute root = new XmlRootAttribute("Categories");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportCategoryDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringWriter writer = new StringWriter(sb);

            serializer.Serialize(writer, exportCategories, namespaces);

            return sb.ToString().TrimEnd();
        }

        // 08 Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportUsersAndProductsDto result = new ExportUsersAndProductsDto()
            {
                Count = context.Users
                .Where(u => u.ProductsSold.Count > 0).Count(),
                Users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new ExportUserDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Products = new ExportSoldProductsDto()
                    {
                        Count = u.ProductsSold.Count,
                        Producst = u.ProductsSold.OrderByDescending(p=>p.Price)
                        .Select(p => new ExportProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).ToArray()

                    }
                })
                .Take(10)
                .ToArray()
            };
            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(ExportUsersAndProductsDto), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, result, namespaces);
            return sb.ToString().TrimEnd();
        }
    }
}