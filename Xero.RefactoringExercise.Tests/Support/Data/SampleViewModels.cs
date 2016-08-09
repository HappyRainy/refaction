using System;
using AutoMapper;
using Xero.RefactoringExercise.Domain.Modles;
using Xero.RefactoringExercise.WebApi.Models;

namespace Xero.RefactoringExercise.Tests.Support.Data
{
    public static class SampleProductViewModels
    {
        public static ProductViewModel SamsungS7ProductModel = new ProductViewModel
        {
            Id = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"),
            DeliveryPrice = 16.99m,
            Price = 1024.99m,
            Name = "Samsung Galaxy S7",
            Description = "Newest mobile product from Samsung."
        };

        public static ProductViewModel IPhone6SProductModel = new ProductViewModel
        {
            Id = new Guid("de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"),
            DeliveryPrice = 15.99m,
            Price = 1299.99m,
            Name = "Apple iPhone 6S",
            Description = "Newest mobile product from Apple."
        };
            	
    }

    public static class SampleProductOptionViewModels
    {
        public static ProductOptionViewModel WhiteGalaxyS7OptionViewModel =
            Mapper.Map<ProductOptionViewModel>(SampleProductOptionDomainModels.WhiteGalaxyS7OptionDomainModel);

        public static ProductOptionViewModel BlackGalaxyS7OptionViewModel =
            Mapper.Map<ProductOptionViewModel>(SampleProductOptionDomainModels.BlackGalaxyS7OptionDomainModel);

        public static ProductOptionViewModel RoseIPhone6SOptionViewModel =
            Mapper.Map<ProductOptionViewModel>(SampleProductOptionDomainModels.RoseIPhone6SOptionDomainModel);

        public static ProductOptionViewModel WhiteIPhone6SOptionViewModel =
            Mapper.Map<ProductOptionViewModel>(SampleProductOptionDomainModels.WhiteIPhone6SOptionDomainModel);

        public static ProductOptionViewModel BlackIPhone6SOptionViewModel =
            Mapper.Map<ProductOptionViewModel>(SampleProductOptionDomainModels.BlackIPhone6SOptionDomainModel);


    }
}
