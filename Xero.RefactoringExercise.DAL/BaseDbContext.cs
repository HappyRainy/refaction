
using System.Data.Entity;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.DAL.Migrations;

namespace Xero.RefactoringExercise.DAL
{
    /// <summary>
    /// Base db context should be only exposed to Domain layer.
    /// </summary>
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(string connectionName) : base(connectionName)
        {
            Database.SetInitializer<BaseDbContext>(null);
            Database.SetInitializer<BaseDbContext>(
                new MigrateDatabaseToLatestVersion<BaseDbContext, Configuration<BaseDbContext>>(connectionName));
        }

        protected DbSet<Product> Products { get; set; }
        protected DbSet<ProductOption> ProductOptions { get; set; }

    }
}
