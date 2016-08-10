using System;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    /// <summary>
    /// Concurrency exception used for indicating the racing condition in db Insert/Update/Delete
    /// </summary>
    public class ConcurrencyException : Exception
    {
    }
}
