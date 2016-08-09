using Xero.RefactoringExercise.DAL;

namespace Xero.RefactoringExercise.Domain
{
    public class XeroRefactoringExerciseContext : BaseDbContext
    {
        public XeroRefactoringExerciseContext () : base("XeroRefactoringExerciseCS")
        {
        }
    }
}
