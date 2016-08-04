using System;

namespace Xero.RefactoringExercise.DAL.Supports
{
    /// <summary>
    /// Entity interface with generice type for id 
    /// </summary>
    /// <typeparam name="T">Type of id</typeparam>
    public interface IEntity<T> : IEntity
    {
        new T Id { get; set; }
    }

    public interface IEntity
    {
        object Id { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
        byte[] RowVersion { get; set; }
    }
}
