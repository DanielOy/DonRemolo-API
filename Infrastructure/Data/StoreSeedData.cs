using Core.Entities;
using Microsoft.AspNetCore.Identity;
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

                if (!context.Doughs.Any())
                {
                    var doughData = File.ReadAllText(path + @"/Data/SeedData/doughs.json");
                    var doughs = JsonSerializer.Deserialize<List<Dough>>(doughData);

                    context.Doughs.AddRange(doughs);
                    await context.SaveChangesAsync();
                }

                if (!context.Sizes.Any())
                {
                    var sizeData = File.ReadAllText(path + @"/Data/SeedData/sizes.json");
                    var sizes = JsonSerializer.Deserialize<List<Size>>(sizeData);

                    context.Sizes.AddRange(sizes);
                    await context.SaveChangesAsync();
                }

                if (!context.Ingredients.Any())
                {
                    var ingredientData = File.ReadAllText(path + @"/Data/SeedData/ingredients.json");
                    var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(ingredientData);

                    context.Ingredients.AddRange(ingredients);
                    await context.SaveChangesAsync();
                }

                if (!context.Promotions.Any())
                {
                    var promotionsData = File.ReadAllText(path + @"/Data/SeedData/promotions.json");
                    var promotions = JsonSerializer.Deserialize<List<Promotion>>(promotionsData);

                    context.Promotions.AddRange(promotions);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreSeedData>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("Administrator").Result)
                {
                    var role = new IdentityRole
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    };

                    await roleManager.CreateAsync(role);
                }

                if (!roleManager.RoleExistsAsync("User").Result)
                {
                    var role = new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    };

                    await roleManager.CreateAsync(role);
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreSeedData>();
                logger.LogError(ex.Message);
            }
        }

        public static async Task SeedUsers(UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (userManager.FindByNameAsync("Admin").Result is null)
                {
                    var user = new User
                    {
                        FullName = "Administrator",
                        UserName = "Admin",
                        Email = "admin@gmail.com"
                    };

                    await userManager.CreateAsync(user, "P@sw0rd*");
                    await userManager.AddToRoleAsync(user, "Administrator");
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
