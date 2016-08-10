using System.Data.Entity;
using Xero.RefactoringExercise.DAL;
using Xero.RefactoringExercise.DAL.Migrations;

namespace Xero.RefactoringExercise.Domain
{
    /// <summary>
    /// This is the db context should be used at upper layer
    /// </summary>
    public class XeroRefactoringExerciseContext : BaseDbContext
    {
        public XeroRefactoringExerciseContext () : base("XeroRefactoringExerciseCS")
        {
            Database.SetInitializer<XeroRefactoringExerciseContext>(null);
            Database.SetInitializer<XeroRefactoringExerciseContext>(
                new MigrateDatabaseToLatestVersion<XeroRefactoringExerciseContext, Configuration<XeroRefactoringExerciseContext>>("XeroRefactoringExerciseCS"));

        }
    }
}
