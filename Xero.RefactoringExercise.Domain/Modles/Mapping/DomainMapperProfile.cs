using AutoMapper;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.Domain.Helpers;

namespace Xero.RefactoringExercise.Domain.Modles.Mapping
{

    /// <summary>
    /// Auto mapper profile for domain layer
    /// </summary>
    public class DomainMapperProfile : Profile
    {
        protected override void Configure()
        {
            base.CreateMap<ProductOption, ProductOptionDomainModel>();
            base.CreateMap<Product, ProductDomainModel>();

            base.CreateMap<ProductDomainModel, Product>()
                .Ignore(record => record.Id)
                .Ignore(record => record.CreatedOn)
                .Ignore(record => record.UpdatedOn);

            base.CreateMap<ProductOptionDomainModel, ProductOption>()
                .Ignore(record => record.Id)
                .Ignore(record => record.CreatedOn)
                .Ignore(record => record.UpdatedOn);
        }
    }
}
