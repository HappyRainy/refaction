using Ninject.Modules;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.Domain
{
    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository>().To<EntityFrameworkRepository<XeroRefactoringExerciseContext>>();
        }
    }
}
