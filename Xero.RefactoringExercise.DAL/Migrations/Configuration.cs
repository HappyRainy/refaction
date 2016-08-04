using System.Collections.Generic;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<XeroRefactoringExerciseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(XeroRefactoringExerciseContext context)
        {
            var products = new List<Product>()
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

            products.ForEach(s => context.Set<Product>().AddOrUpdate(s));
            context.SaveChanges();

            productOptions.ForEach(s => context.Set<ProductOption>().AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
