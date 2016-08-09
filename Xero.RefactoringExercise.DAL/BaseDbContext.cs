
using System.Data.Entity;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.DAL.Migrations;

namespace Xero.RefactoringExercise.DAL
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(string connectionName) : base(connectionName)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BaseDbContext, Configuration<BaseDbContext>>(connectionName));
        }

        protected DbSet<Product> Products { get; set; }
        protected DbSet<ProductOption> ProductOptions { get; set; }

    }
}
