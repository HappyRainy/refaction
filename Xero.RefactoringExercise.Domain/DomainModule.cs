using AutoMapper;
using Ninject.Modules;
using Xero.RefactoringExercise.DAL.Supports;
using Xero.RefactoringExercise.Domain.Modles.Mapping;
using Xero.RefactoringExercise.Domain.Services;

namespace Xero.RefactoringExercise.Domain
{
    /// <summary>
    /// Ninject module for domain layer
    /// </summary>
    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<EntityFrameworkRepository<XeroRefactoringExerciseContext>>();

            Bind<IProductService>().To<ProductService>();
            Bind<IProductOptionService>().To<ProductOptionService>();
            Bind<IUserContextService>().To<SimpleUserContextService>();

            DefineMapper();
        }

        public static void DefineMapper()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new DomainMapperProfile());
            });
        }
    }
}
