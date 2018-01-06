using System;
using FastFood.Data;
using System.Linq;
using System.Text;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
	    public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
	    {
            var findItem = context.Items.SingleOrDefault(i => i.Name == itemName);

            if (findItem == null)
            {
                return $"Item {itemName} not found!";                 
            }

            var oldPrice = findItem.Price;

            findItem.Price = newPrice;
            context.SaveChanges();

            return $"{itemName} Price updated from ${oldPrice:F2} to ${newPrice:F2}";
        }
    }
}
