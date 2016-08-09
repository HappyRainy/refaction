using System;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    public class UniqueConstraintException : Exception
    {
        public UniqueConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
