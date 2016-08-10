using System;

namespace Xero.RefactoringExercise.Domain.Exceptions
{
    /// <summary>
    /// Record not found exception used for indicating the record dosn't exist in db within domain layer
    /// </summary>
    public class RecordNotFoundException : Exception
    {
    }
}
