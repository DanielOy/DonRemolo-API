using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;

namespace Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<User>
    {
        private const string _sqliteProviderName = "Microsoft.EntityFrameworkCore.Sqlite";

        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            if (Database.ProviderName == _sqliteProviderName)
            {
                ApplySqliteConfigurations(builder);
            }
        }

        private static void ApplySqliteConfigurations(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));

                var dateTimeProperties = entityType.ClrType
                    .GetProperties()
                    .Where(x => x.PropertyType == typeof(DateTimeOffset));

                foreach (var property in properties)
                {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<double>();
                }

                foreach (var property in dateTimeProperties)
                {
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }

        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Dough> Doughs { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
        public DbSet<BasketIngredient> BasketIngredients { get; set; }
    }
}
