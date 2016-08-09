using System;

namespace Xero.RefactoringExercise.DAL.Supports
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        byte[] RowVersion { get; set; }
    }
}
