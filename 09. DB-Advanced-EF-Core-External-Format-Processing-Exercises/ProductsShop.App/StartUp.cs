
namespace ProductsShop.App
{
    using System;
    using ProductsShop.Data;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using ProductsShop.Models;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Text;
    using ProductsShop.App.Files;

    class StartUp
    {
        static void Main(string[] args)
        {
          //  ResetDB();
            // Files are exported in the ProductsShop.App directory.
        //    ImprotExportJson();
            ImportExportXml();
        }

        private static void ImportExportXml()
        {
            //Console.WriteLine(ImportUsersXML());
            //Console.WriteLine(ImportCategoriesXML());
            //Console.WriteLine(ImportProductsXML());
            //Console.WriteLine(GetProductsInRangeXML());
            Console.WriteLine(GetSoldProductsXML());
            //Console.WriteLine(GetCategoriesByProductsCountXML());
            //Console.WriteLine(GetUsersAndProductsXML());
        }

        //private static void ImprotExportJson()
        //{
        //    Console.WriteLine(ImportUsersFromJson());
        //    Console.WriteLine(ImportCategoriesFromJson());
        //    Console.WriteLine(ImportProductsFromJson());
        //    Console.WriteLine(SetCategories());
        //    Console.WriteLine(GetProductsInRange());
        //    Console.WriteLine(GetSoldProducts());
        //    Console.WriteLine(GetCategoriesByProductsCount());
        //    Console.WriteLine(GetUsersandProducts()); ;
        //}

        private static void ResetDB()
        {
            var context = new ProductsShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        // IMPORT DATA JSON
        static string ImportUsersFromJson()
        {
            var path = "Files/users.json";
            User[] users = ImportJson<User>(path);

            using (var context = new ProductsShopContext())
            {
                context.Users.AddRange(users);

                context.SaveChanges();
            }

            return $"{users.Length} users were successfuly imported from {path}";
        }

        static string ImportCategoriesFromJson()
        {
            var path = "Files/categories.json";

            var categories = ImportJson<Category>(path);

            using (var context = new ProductsShopContext())
            {
                context.AddRange(categories);
                context.SaveChanges();
            }
            return $"{categories.Length} categories were successfuly imported from {path}";

        }

        static string ImportProductsFromJson()
        {
            var path = "Files/products.json";

            var rnd = new Random();

            var products = ImportJson<Product>(path);

            using (var context = new ProductsShopContext())
            {
                var userIds = context.Users.Select(u => u.Id).ToArray();


                foreach (var p in products)
                {
                    var sellerIndex = rnd.Next(0, userIds.Length);
                    var sellerId = userIds[sellerIndex];

                    int? buyerId = sellerId;
                    while (buyerId == sellerId)
                    {
                        int buyerIndex = rnd.Next(0, userIds.Length);
                        buyerId = userIds[buyerIndex];

                    }
                    if (buyerId > sellerId)
                    {
                        buyerId = null;
                    }
                    p.SellerId = sellerId;
                    p.BuyerId = buyerId;

                }

                context.AddRange(products);
                context.SaveChanges();
            }
            return $"{products.Length} products were successfuly imported from {path}";

        }

        static string SetCategories()
        {
            using (var context = new ProductsShopContext())
            {
                var productIds = context.Products.Select(p => p.Id).ToArray();

                var categoryIds = context.Categories.Select(c => c.Id).ToArray();

                var categoryProducts = new List<CategoryProduct>();

                var rnd = new Random();

                foreach (var p in productIds)
                {
                    var catProducts = new List<CategoryProduct>();
                    for (int i = 0; i < 3; i++)
                    {

                        int catIndex = rnd.Next(0, categoryIds.Length);

                        var categoryProd = new CategoryProduct()
                        {
                            ProductId = p,
                            CategoryId = categoryIds[catIndex]

                        };
                        if (catProducts.Select(c => c.CategoryId).Contains(categoryProd.CategoryId))
                        {
                            i--;
                            continue;
                        }
                        catProducts.Add(categoryProd);
                    }
                    context.CategoryProducts.AddRange(catProducts);
                    context.SaveChanges();
                }
            }
            return $"Categories updated successfuly";
        }

        static T[] ImportJson<T>(string path)
        {
            string jsonString = File.ReadAllText(path);

            var objects = JsonConvert.DeserializeObject<T[]>(jsonString);

            return objects;
        }

        //EXPORT DATA JSON

        static string GetProductsInRange()
        {
            using (var context = new ProductsShopContext())
            {
                var products = context.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        seller = $"{p.Seller.FirstName} {p.Seller.LastName}"

                    }).ToArray();

                var jsonString = JsonConvert.SerializeObject(products, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText("GetProductsInRange.json", jsonString);
                return $"Data exported to JSON successfully";
            }
        }

        static string GetSoldProducts()
        {
            using (var context = new ProductsShopContext())
            {
                var users = context.Users.Where(p => p.ProductsSold.Any(b => b.BuyerId != null))
                    .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        SoldProducts = u.ProductsSold.Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = $"{p.Buyer.FirstName}",
                            BuyerLastName = $"{p.Buyer.LastName}"
                        })
                    }).ToArray();



                var jsonString = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });

                File.WriteAllText("GetSoldProducts.json", jsonString);
                return $"Data exported to JSON successfully";

            }
        }

        static string GetCategoriesByProductsCount()
        {
            using (var context = new ProductsShopContext())
            {
                var categories = context.Categories.OrderBy(c => c.Name)
                    .Select(c => new
                    {
                        category = c.Name,
                        productsCount = c.Products.Count,
                        averagePrice = c.Products.Average(p => p.Product.Price),
                        totalRevenue = c.Products.Sum(p => p.Product.Price)
                    }).ToArray();

                var jsonString = JsonConvert.SerializeObject(categories, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("GetCategoriesByProductsCount.json", jsonString);
                return $"Data exported to JSON successfully";

            }
        }

        static string GetUsersandProducts()
        {
            using (var context = new ProductsShopContext())
            {
                var usersCount = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null)).Count();

                var soldProducts = context
                  .Products
                    .Include(p => p.Seller)
                      .Where(p => p.BuyerId != null)
                        .ToArray();

                var users = new
                {
                    usersCount = usersCount,
                    users = soldProducts.Select(sp => new
                    {
                        firstName = sp.Seller.FirstName,
                        lastName = sp.Seller.LastName,
                        age = sp.Seller.Age,
                        soldProducts = new
                        {
                            count = sp.Seller.ProductsSold.Count,
                            products = sp.Seller.ProductsSold.Select(pr => new
                            {
                                pr.Name,
                                pr.Price
                            }).ToArray()
                        }

                    }).ToArray()
                };

                var jsonString = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented,
             new JsonSerializerSettings
             {
                 DefaultValueHandling = DefaultValueHandling.Ignore
             });

                File.WriteAllText("GetUsersandProducts.json", jsonString);
                return $"Data exported to JSON successfully";

            }
        }

        // IMPORT DATA XML

        static string ImportUsersXML()
        {
            string xmlStr = File.ReadAllText("Files/users.xml");

            var xmlDoc = XDocument.Parse(xmlStr);

            var elements = xmlDoc.Root.Elements();

            var users = new List<User>();

            foreach (var element in elements)
            {
                var firstName = element.Attribute("firstName")?.Value; // ? Takes care of nulls

                var lastame = element.Attribute("lastName").Value;

                int? age = null;

                if (element.Attribute("age") != null)
                {
                    age = int.Parse(element.Attribute("age").Value); //("age.value") returns "Not set to an instance of an object"
                }

                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastame,
                    Age = age
                };

                users.Add(user);
            }
            using (var context = new ProductsShopContext())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return $"{users.Count} users imported succesfully from Files/users.xml";
        }

        static string ImportCategoriesXML()
        {
            var path = "Files/categories.xml";

            var xmlString = File.ReadAllText(path);

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var categories = new List<Category>();

            foreach (var element in elements)
            {
                var category = new Category()
                {
                    Name = element.Element("name").Value
                };

                categories.Add(category);
            }

            using (var context = new ProductsShopContext())
            {
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            return $"{categories.Count} categories imported succesfully from {path}";
        }

        static string ImportProductsXML()
        {
            var path = "Files/products.xml";

            var xmlString = File.ReadAllText(path);

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var catProducts = new List<CategoryProduct>();

            using (var context = new ProductsShopContext())
            {
                var rnd = new Random();
                var userIds = context.Users.Select(u => u.Id).ToArray();
                var categoryIds = context.Categories.Select(u => u.Id).ToArray();

                foreach (var e in elements)
                {
                    var name = e.Element("name").Value;
                    decimal price = decimal.Parse(e.Element("price").Value);

                    var sellerIndex = rnd.Next(0, userIds.Length);
                    var sellerId = userIds[sellerIndex];

                    var categoryIndex = rnd.Next(0, categoryIds.Length);
                    var categoryId = categoryIds[categoryIndex];

                    int? buyerId = sellerId;
                    while (buyerId == sellerId)
                    {
                        int buyerIndex = rnd.Next(0, userIds.Length);
                        buyerId = userIds[buyerIndex];

                    }
                    if (buyerId > sellerId)
                    {
                        buyerId = null;
                    }

                    var product = new Product()
                    {
                        Name = name,
                        Price = price,
                        BuyerId = buyerId,
                        SellerId = sellerId,
                    };


                    var categoryProduct = new CategoryProduct
                    {
                        Product = product,
                        CategoryId = categoryId
                    };

                    catProducts.Add(categoryProduct);
                }
                context.AddRange(catProducts);
                context.SaveChanges();
            }
            return $"{catProducts.Count} products imported successfully from {path}";
        }


        //EXPORT XML
        static string GetProductsInRangeXML()
        {
            using (var context = new ProductsShopContext())
            {
                var exportDir = "./Files/";
                var products = context.Products.Where(p => p.Price >= 1000 && p.Price <= 2000)
                    .Include(p => p.Buyer).Select(e => new
                    {
                        buyerid = e.BuyerId,
                        productName = e.Name,
                        price = e.Price,
                        buyer = $"{e.Buyer.FirstName} {e.Buyer.LastName}"
                    }).ToArray();
                var result = products.Where(p => p.buyerid != null)
                    .Select(e => new ProductDto
                {
                        Name = e.productName,
                        Price = e.price,
                        Buyer = e.buyer
                }).ToArray();

                var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));

                var text = new StringBuilder();

                serializer.Serialize(new StringWriter(text), result, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

                File.WriteAllText(exportDir + "GetProductsInRange.xml", text.ToString());
            }

            return $"Data exported to Xml successfully";

        }

        static string GetSoldProductsXML()
        {
            using (var context = new ProductsShopContext())
            {
                var exportDir = "./Files/";

                var users = context.Users
                    .Where(u => u.ProductsSold.Count >= 1)
                    .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                    .Select(u => new SoldProductsDto
                    {
                        firstname = u.FirstName,
                        lastname = u.LastName,
                        soldproducts = u.ProductsSold.Select(p => new ProductSoldDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).ToList()

                    }).ToArray();

                var sb = new StringBuilder();

                var serializer = new XmlSerializer(typeof(SoldProductsDto[]), new XmlRootAttribute("users"));
                serializer.Serialize(new StringWriter(sb), users, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

                var result = sb.ToString();
                File.WriteAllText(exportDir + "Test.xml", result.ToString());
            }

            return $"Data exported to Xml successfully";

        }

        static string GetCategoriesByProductsCountXML()
        {
            using (var context = new ProductsShopContext())
            {
                var categories = context.Categories.OrderByDescending(c => c.Products.Count)
                    .Select(c => new
                    {
                        name = c.Name,
                        numberOfProducts = c.Products.Count,
                        averagePrice = c.Products.Average(p => p.Product.Price),
                        totalRevenue = c.Products.Sum(p => p.Product.Price)
                    }).ToArray();

                var xDoc = new XDocument(new XElement("categories"));
                foreach (var c in categories)
                {
                    xDoc.Root.Add(new XElement("category",
                   new XAttribute("name", c.name),
                    new XElement("products-count", c.numberOfProducts),
                   new XElement("average-price", c.averagePrice),
                    new XElement("total-revenue", c.totalRevenue)));
                }

                xDoc.Save("GetCategoriesByProductsCountXML.xml");
            }

            return $"Data exported to Xml successfully";
        }

        static string GetUsersAndProductsXML()
        {
            using (var context = new ProductsShopContext())
            {
                var users = context.Users.Where(u => u.ProductsSold.Count >= 1)
                    .OrderByDescending(u => u.ProductsSold.Count).ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        soldProductsCount = u.ProductsSold.Count,
                        productsSold = u.ProductsSold.ToArray()
                    }).ToArray();

                var xDoc = new XDocument(new XElement("users-count", 
                    new XAttribute("count", users.Length)));

                foreach (var u in users)
                {
                    var firstName = u.firstName;
                    var lastName = u.lastName;
                    var age = u.age;

                    var productsSoldXml = new XElement("sold-products",
                        new XAttribute("count", u.soldProductsCount));
                    
        
                    foreach (var p in u.productsSold)
                    {
                        var name = p.Name;
                        var price = p.Price;

                        var currXel = new XElement("product",
                            new XAttribute("name", name),
                            new XAttribute("price", price));

                        productsSoldXml.Add(currXel);
                    }

                    var userXel = (new XElement("user"));

                    if (u.firstName != null)
                    {
                        userXel.Add(new XAttribute("first-name", firstName));
                    }
                    userXel.Add(new XAttribute("last-name", lastName));
                    if (age != null)
                    {
                        userXel.Add(new XAttribute("age", age));
                    }
                
                    userXel.Add(productsSoldXml);

                    xDoc.Root.Add(userXel);
                }
                xDoc.Save("GetUsersAndProductsXML.xml");
            }
            return $"Data exported to Xml successfully";
        }
    }
}
