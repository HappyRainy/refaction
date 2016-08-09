using AutoMapper;
using Xero.RefactoringExercise.Domain.Modles;

namespace Xero.RefactoringExercise.Tests.Support.Data
{

    public static class SampleProductDomainModels
    {
        public static ProductDomainModel SamsungS7ProductDomainModel =
            Mapper.Map<ProductDomainModel>(SampleProducts.SamsungS7Product);

        public static ProductDomainModel IPhone6SProductDomainModel =
            Mapper.Map<ProductDomainModel>(SampleProducts.IPhone6SProduct);

        public static ProductDomainModel SamsungS6ProductDomainModel =
            Mapper.Map<ProductDomainModel>(SampleProducts.SamsungS6Product);
    }

    public static class SampleProductOptionDomainModels
    {
        public static ProductOptionDomainModel WhiteGalaxyS7OptionDomainModel =
            Mapper.Map<ProductOptionDomainModel>(SampleProductOptions.WhiteGalaxyS7);

        public static ProductOptionDomainModel BlackGalaxyS7OptionDomainModel =
            Mapper.Map<ProductOptionDomainModel>(SampleProductOptions.BlackGalaxyS7);

        public static ProductOptionDomainModel RoseIPhone6SOptionDomainModel =
            Mapper.Map<ProductOptionDomainModel>(SampleProductOptions.RoseIPhone6S);

        public static ProductOptionDomainModel WhiteIPhone6SOptionDomainModel =
            Mapper.Map<ProductOptionDomainModel>(SampleProductOptions.WhiteIPhone6S);

        public static ProductOptionDomainModel BlackIPhone6SOptionDomainModel =
            Mapper.Map<ProductOptionDomainModel>(SampleProductOptions.BlackIPhone6S);
    }
}