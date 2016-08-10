using System;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    /// <summary>
    /// Unique constraint exception used to indicate the voilation of unique index of the table while db update/insert
    /// </summary>
    public class UniqueConstraintException : Exception
    {
        public UniqueConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
