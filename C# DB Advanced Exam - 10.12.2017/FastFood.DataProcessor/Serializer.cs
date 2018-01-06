using System;
using System.IO;
using FastFood.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using FastFood.DataProcessor.Dto.Export;
using System.Xml;

namespace FastFood.DataProcessor
{
	public class Serializer
	{
		public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
		{
            var employee = context.Employees
                .Where(e => e.Name == employeeName && e.Orders.Any(order => order.Type.ToString() == orderType))
                .Select(e => new
                {
                    Name = e.Name,
                    Orders = e.Orders
                    .Where(order => order.Type.ToString() == orderType)
                    .Select(order => new
                    {
                        Customer = order.Customer,
                        Items = order.OrderItems    
                        .Select(orderitem => new
                        {
                            Name = orderitem.Item.Name,
                            Price = orderitem.Item.Price,
                            Quantity = orderitem.Quantity
                        })
                        .ToArray(),
                        TotalPrice = order.OrderItems.Sum(orderitem => orderitem.Quantity * orderitem.Item.Price)
                    })
                    .OrderByDescending(order => order.TotalPrice)
                    .ThenByDescending(order => order.Items.Count())
                    .ToArray(),
                    TotalMade = e.Orders.Sum(order => order.OrderItems.Sum(orderitem => orderitem.Quantity * orderitem.Item.Price))
                })
                .First();

            var result = JsonConvert.SerializeObject(employee, Newtonsoft.Json.Formatting.Indented);

            return result;
        }

		public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
		{
            var inputCategories = categoriesString.Split(",");

            var categories = context.Categories
                  .Where(c => inputCategories.Contains(c.Name))
                  .Select(c => new CategoriesDto
                  {
                      Name = c.Name,
                      MostPopularItem = c.Items
                      .OrderByDescending(i => i.Price * i.OrderItems.Sum(oa => oa.Quantity))
                      .Select(mp => new MostPopularItemDto
                      {
                          Name = mp.Name,
                          TotalMade = mp.Price * mp.OrderItems.Sum(oa => oa.Quantity),
                          TimesSold = mp.OrderItems.Sum(mpoa => mpoa.Quantity)
                      })
                      .FirstOrDefault()
                  })
                  .OrderByDescending(a => a.MostPopularItem.TotalMade)
                  .ThenByDescending(a => a.MostPopularItem.TimesSold)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CategoriesDto[]), new XmlRootAttribute("Categories"));
            serializer.Serialize(new StringWriter(sb), categories, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));


            var result = sb.ToString();
            return result;
        }
	}
}