using System;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    public class DatabaseAccessException : Exception
    {
        public DatabaseAccessException(string message, Exception innerException) 
            : base(message, innerException)
        {
            
        }
    }
}
