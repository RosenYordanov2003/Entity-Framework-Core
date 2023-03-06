namespace ProductShop
{
    using System;
    using ProductShop.Data;
    using ProductShop.Dtos.Import;
    using ProductShop.Models;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    public class StartUp
    {
        private const string MainPath = "../../../Datasets/";
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            string inputXmlFile = File.ReadAllText($"{MainPath}categories-products.xml");
            Console.WriteLine(ImportCategoryProducts(context,inputXmlFile));
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
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductsDto[]),root);

            using StringReader reader = new StringReader(inputXml);

            ImportProductsDto[]products = (ImportProductsDto[])serializer.Deserialize(reader);

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
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryProductDto), root);

            StringReader reader = new StringReader(inputXml);

            ImportCategoryProductDto[] productsCategories = (ImportCategoryProductDto[])serializer.Deserialize(reader);

            CategoryProduct[] categoryProductsToAdd = productsCategories
                .Select(cp => new CategoryProduct
                {
                    CategoryId = cp.CategoryId,
                    ProductId = cp.ProductId
                }).ToArray();
            context.CategoryProducts.AddRange(categoryProductsToAdd);
            context.SaveChanges();
            return $"Successfully imported {categoryProductsToAdd.Length}";
        }
    }
}