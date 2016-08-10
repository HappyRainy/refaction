using AutoMapper;
using Ninject.Modules;
using Ninject.Web.Common;
using Xero.RefactoringExercise.Domain;
using Xero.RefactoringExercise.Domain.Context;
using Xero.RefactoringExercise.Domain.Modles.Mapping;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.WebApi.Infrastructure;
using Xero.RefactoringExercise.WebApi.Models.Mapping;

namespace Xero.RefactoringExercise.WebApi
{
    public class WebApiModule : NinjectModule
    {
        public override void Load()
        {
            Bind<XeroRefactoringExerciseContext>().ToSelf().InRequestScope();
            Bind<AppContext, WebApiAppContext>().To<WebApiAppContext>().InRequestScope();

            Rebind<IUserContextService>().To<SimpleUserContextService>().InRequestScope();

            DefineMapper();
        }

        public static void DefineMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new DomainMapperProfile());
                cfg.AddProfile(new WebApiMapperProfile());
            });
        }
    }
}
