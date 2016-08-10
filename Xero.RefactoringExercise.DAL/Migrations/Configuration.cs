using System;
using System.Collections.Generic;
using System.Data.Entity;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration<TDbContext> : DbMigrationsConfiguration<TDbContext>
        where TDbContext : DbContext
    {
        private IRepository _repository;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TDbContext context)
        {
            _repository = new EntityFrameworkRepository<TDbContext>(context);

            if (_repository.GetCount<Product>() != 0) return;

            var products = new List<Product>()
            {
                new Product()
                {
                    Name = "Samsung Galaxy S7",
                    Description = "Newest mobile product from Samsung.",
                    Price = 1024.99m,
                    DeliveryPrice = 16.99m,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                },
                new Product()
                {
                    Name = "Apple iPhone 6S",
                    Description = "Newest mobile product from Apple.",
                    Price = 1299.99m,
                    DeliveryPrice = 15.99m,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                }
            };
            var productOptions = new List<ProductOption>()
            {
                new ProductOption()
                {
                    Name = "White",
                    Product = products[0],
                    Description = "White Samsung Galaxy S7",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                },
                new ProductOption()
                {
                    Name = "Black",
                    Product = products[0],
                    Description = "Black Samsung Galaxy S7",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                },
                new ProductOption()
                {
                    Name = "Rose",
                    Product = products[1],
                    Description = "Gold Gold Apple iPhone 6S",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                },
                new ProductOption()
                {
                    Name = "White",
                    Product = products[1],
                    Description = "White Apple iPhone 6S",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                },
                new ProductOption()
                {
                    Name = "Black",
                    Product = products[1],
                    Description = "Black Apple iPhone 6S",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "InitData"
                }
            };

            products.ForEach(s => context.Set<Product>().AddOrUpdate(s));
            context.SaveChanges();

            productOptions.ForEach(s => context.Set<ProductOption>().AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
