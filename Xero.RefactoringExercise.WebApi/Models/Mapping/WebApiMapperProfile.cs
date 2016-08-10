using AutoMapper;
using Xero.RefactoringExercise.Domain.Helpers;
using Xero.RefactoringExercise.Domain.Modles;

namespace Xero.RefactoringExercise.WebApi.Models.Mapping
{
    /// <summary>
    /// Auto mapper profile for web api 
    /// </summary>
    public class WebApiMapperProfile : Profile
    {
        protected override void Configure()
        {
            base.CreateMap<ProductDomainModel, ProductViewModel>();
            base.CreateMap<ProductViewModel, ProductDomainModel>()
                .Ignore(record => record.Id);

            base.CreateMap<ProductOptionDomainModel, ProductOptionViewModel>();
            base.CreateMap<ProductOptionViewModel, ProductOptionDomainModel>()
                .Ignore(record => record.Id);

        }
    }
}
