using System;
using System.Collections.Generic;
using Xero.RefactoringExercise.DAL.Entities;

namespace Xero.RefactoringExercise.Tests.Support.Data
{
    public static class SampleProductOptions
    {
        public static ProductOption WhiteGalaxyS7 = new ProductOption()
        {
            Id = new Guid("0643ccf0-ab00-4862-b3c5-40e2731abcc9"),
            ProductId = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = null,
            CreatedBy = "Jing",
            UpdatedBy = string.Empty,
            Name = "White",
            Description = "White Samsung Galaxy S7"
        };

        public static ProductOption BlackGalaxyS7 = new ProductOption()
        {
            Id = new Guid("a21d5777-a655-4020-b431-624bb331e9a2"),
            ProductId = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = null,
            CreatedBy = "Jing",
            UpdatedBy = string.Empty,
            Name = "Black",
            Description = "Black Samsung Galaxy S7"
        };

        public static ProductOption RoseIPhone6S = new ProductOption()
        {
            Id = new Guid("5c2996ab-54ad-4999-92d2-89245682d534"),
            ProductId = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
            CreatedOn = DateTime.UtcNow.AddDays(-2),
            UpdatedOn = DateTime.UtcNow.AddDays(-1),
            CreatedBy = "Jing",
            UpdatedBy = "Jing",
            Name = "Rose",
            Description = "Gold Gold Apple iPhone 6S"
        };

        public static ProductOption WhiteIPhone6S = new ProductOption()
        {
            Id = new Guid("9ae6f477-a010-4ec9-b6a8-92a85d6c5f03"),
            ProductId = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
            CreatedOn = DateTime.UtcNow.AddDays(-2),
            UpdatedOn = DateTime.UtcNow.AddDays(-1),
            CreatedBy = "Jing",
            UpdatedBy = "Jing",
            Name = "White",
            Description = "White Apple iPhone 6S"
        };

        public static ProductOption BlackIPhone6S = new ProductOption()
        {
            Id = new Guid("4e2bc5f2-699a-4c42-802e-ce4b4d2ac0ef"),
            ProductId = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
            CreatedOn = DateTime.UtcNow.AddDays(-2),
            UpdatedOn = DateTime.UtcNow,
            CreatedBy = "Jing",
            UpdatedBy = "TestUser",
            Name = "Black",
            Description = "Black Apple iPhone 6S"
        };
    }

    public static class SampleProducts
    {
        public static Product SamsungS7Product = new Product
        {
            Id = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = null,
            CreatedBy = "Jing",
            UpdatedBy = string.Empty,
            DeliveryPrice = 16.99m,
            Price = 1024.99m,
            Name = "Samsung Galaxy S7",
            Description = "Newest mobile product from Samsung.",
            RowVersion = BitConverter.GetBytes(0x00000000000007D1),
            ProductOptions = new List<ProductOption>
            {
                SampleProductOptions.BlackGalaxyS7,
                SampleProductOptions.WhiteGalaxyS7
            }
        };

        public static Product SamsungS6Product = new Product
        {
            Id = new Guid("8f2e9176-35ee-730a-ae55-83023d2db1a3"),
            CreatedOn = DateTime.UtcNow.AddYears(-1),
            UpdatedOn = DateTime.UtcNow.AddDays(-1),
            CreatedBy = "Jing",
            UpdatedBy = "Jing",
            DeliveryPrice = 15.00m,
            Price = 999.99m,
            Name = "Samsung Galaxy S6",
            Description = "Very awesome product from Samsung.",
            RowVersion = BitConverter.GetBytes(0x00000000000007D1)
        };

        public static Product IPhone6SProduct = new Product
        {
            Id = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
            CreatedOn = DateTime.UtcNow.AddDays(-2),
            UpdatedOn = DateTime.UtcNow.AddDays(-1),
            CreatedBy = "Jing",
            UpdatedBy = "Jing",
            DeliveryPrice = 15.99m,
            Price = 1299.99m,
            Name = "Apple iPhone 6S",
            Description = "Newest mobile product from Apple.",
            RowVersion = BitConverter.GetBytes(0x00000000000007D0),
            ProductOptions = new List<ProductOption>
            {
                SampleProductOptions.BlackIPhone6S,
                SampleProductOptions.RoseIPhone6S,
                SampleProductOptions.WhiteIPhone6S
            }
        };
    }
}
