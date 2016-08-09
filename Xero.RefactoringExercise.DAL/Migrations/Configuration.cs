using System.Data.Entity;

namespace Xero.RefactoringExercise.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration<TDbContext> : DbMigrationsConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TDbContext context)
        {
            /*var products = new List<Product>()
            {
                new Product()
                {
                    Name = "Samsung Galaxy S7",
                    Description = "Newest mobile product from Samsung.",
                    Price = 1024.99m,
                    DeliveryPrice = 16.99m
                },
                new Product() {Name = "Apple iPhone 6S", Description = "Newest mobile product from Apple.", Price = 1299.99m, DeliveryPrice = 15.99m},
            };

            var productOptions = new List<ProductOption>()
            {
                new ProductOption() {Name = "White", Product = products[0], Description = "White Samsung Galaxy S7"},
                new ProductOption() {Name = "Black", Product = products[0], Description = "Black Samsung Galaxy S7"},
                new ProductOption() {Name = "Rose", Product = products[1], Description = "Gold Gold Apple iPhone 6S"},
                new ProductOption() {Name = "White", Product = products[1], Description = "White Apple iPhone 6S"},
                new ProductOption() {Name = "Black", Product = products[1], Description = "Black Apple iPhone 6S"}
            };

            products.ForEach(s => context.Set<Product>().Add(s));
            context.SaveChanges();

            productOptions.ForEach(s => context.Set<ProductOption>().Add(s));
            context.SaveChanges();*/
        }
    }
}
