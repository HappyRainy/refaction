using Ninject.Modules;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.DAL
{
    public class XeroRefactoringExerciseDALModule : NinjectModule
    {
        public override void Load()
        {
            Bind<XeroRefactoringExerciseContext>()
                     .ToSelf();

            Bind<IRepository>()
                  .To<XeroRefactoringExerciseRepository<XeroRefactoringExerciseContext>>();
        }
    }
}
