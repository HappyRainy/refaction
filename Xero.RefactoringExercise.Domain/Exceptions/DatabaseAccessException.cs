using System;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    /// <summary>
    /// Generic db access exception
    /// </summary>
    public class DatabaseAccessException : Exception
    {
        public DatabaseAccessException(string message, Exception innerException) 
            : base(message, innerException)
        {
            
        }
    }
}
