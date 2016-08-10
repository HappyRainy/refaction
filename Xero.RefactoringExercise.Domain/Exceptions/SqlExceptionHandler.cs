using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    /// <summary>
    /// Sql exception handler to help throwing proper exceptions to upper layer
    /// </summary>
    public static class SqlExceptionHandler
    {
        public static void HandleException(Exception exception)
        {
            var concurrencyEx = exception as DbUpdateConcurrencyException;
            if (concurrencyEx != null)
            {
                throw new ConcurrencyException();
            }

            var dbUpdateEx = exception as DbUpdateException;
            if (dbUpdateEx?.InnerException?.InnerException == null) return;

            var sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
            if (sqlException == null) throw new DatabaseAccessException(dbUpdateEx.InnerException.Message, dbUpdateEx.InnerException);

            switch (sqlException.Number)
            {
                case 2627:  // Unique constraint error
                case 547:   // Constraint check violation
                case 2601:  // Duplicated key row error
                    // Constraint violation exception
                    throw new UniqueConstraintException(sqlException.Message, sqlException);

                default:
                    throw new DatabaseAccessException(sqlException.Message, sqlException);
            }
        }
    }
}
