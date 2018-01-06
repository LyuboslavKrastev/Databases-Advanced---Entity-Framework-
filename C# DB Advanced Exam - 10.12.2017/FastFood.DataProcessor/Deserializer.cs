using System;
using FastFood.Data;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;
using FastFood.Models.Enums;
using AutoMapper;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Xml.Linq;

namespace FastFood.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            var deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var sb = new StringBuilder();

            var validEmployees = new List<Employee>();

            foreach (var employeeDto in deserializedEmployees)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!context.Positions.Any(p => p.Name == employeeDto.Position))
                {
                    var position = new Position()
                    {
                        Name = employeeDto.Position
                    };

                    if (!IsValid(position))
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    context.Positions.Add(position);
                    context.SaveChanges();
                }          

                var employee = new Employee()
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = context.Positions.SingleOrDefault(p => p.Name == employeeDto.Position)
                };

                sb.AppendLine(string.Format(SuccessMessage, employeeDto.Name));
                validEmployees.Add(employee);
            }
            context.Employees.AddRange(validEmployees);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            var deserializedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            var sb = new StringBuilder();

            var validItems = new List<Item>();

            foreach (var itemDto in deserializedItems)
            {
                if (!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                } 
                // item exists
                if (validItems.Any(i => i.Name == itemDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                if (!context.Categories.Any(c => c.Name == itemDto.Category))
                {
                    var category = new Category()
                    {
                        Name = itemDto.Category
                    };
                    if (!IsValid(category))
                    {
                        sb.AppendLine(FailureMessage);
                        continue;
                    }

                    context.Categories.Add(category);
                    context.SaveChanges();
                }
              
            var currItem = new Item()
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
                Category = context.Categories.SingleOrDefault(c => c.Name == itemDto.Category)
            };

            validItems.Add(currItem);
            sb.AppendLine(String.Format(SuccessMessage, currItem.Name));
        }
        context.Items.AddRange(validItems);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
    }

    public static string ImportOrders(FastFoodDbContext context, string xmlString)
    {

        var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));

        var deserializedOrders = (OrderDto[])serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString)));

        var sb = new StringBuilder();

        var validOrders = new List<Order>();

        var validOrderItems = new List<OrderItem>();

        foreach (var orderDto in deserializedOrders)
        {
            if (!IsValid(orderDto))
            {
                sb.AppendLine(FailureMessage);
                continue;
            }

            var orderDateTime = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var employee = context.Employees.FirstOrDefault(e => e.Name == orderDto.Employee);
            var itemsExist = orderDto.Items.All(i => context.Items.Any(ci => ci.Name == i.Name));

            if (employee == null || !itemsExist)
            {
                sb.AppendLine(FailureMessage);
                continue;
            }

            decimal totalPrice = 0.0m;

            foreach (var item in orderDto.Items)
            {
                var currentItemPrice = context.Items.SingleOrDefault(i => i.Name == item.Name).Price;
                totalPrice += (decimal)item.Quantity * (decimal)currentItemPrice;
            }

            var currentOrder = new Order()
            {
                Customer = orderDto.Customer,
                DateTime = orderDateTime,
                Employee = employee,
                Type = Enum.Parse<OrderType>(orderDto.Type),
                TotalPrice = totalPrice
            };

            validOrders.Add(currentOrder);
            sb.AppendLine($"Order for {orderDto.Customer} on {orderDto.DateTime} added");

            context.Orders.Add(currentOrder);
            context.SaveChanges();

            foreach (var item in orderDto.Items)
            {
                validOrderItems.Add(new OrderItem()
                {
                    ItemId = context.Items.First(i => i.Name == item.Name).Id,
                    Quantity = item.Quantity,
                    OrderId = context.Orders.Last().Id
                });
            }
        }
        context.OrderItems.AddRange(validOrderItems);
        context.SaveChanges();
        return sb.ToString().TrimEnd();
    }

    private static bool IsValid(object obj)
    {
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
        var validationResults = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
        return isValid;
    }
}
}