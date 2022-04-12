using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class StoreSeedData
    {
        public static async Task SeedInitialData(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.Categories.Any())
                {
                    var categorieData = File.ReadAllText(path + @"/Data/SeedData/categories.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categorieData);

                    context.Categories.AddRange(categories);
                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreSeedData>();
                logger.LogError(ex.Message);
            }
        }
    }
}
