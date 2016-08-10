using System;

namespace Xero.RefactoringExercise.Domain.Modles
{
    /// <summary>
    /// Model base for domain layer, the realy db entity object should be hidden all the time to upper layer 
    /// </summary>
    public abstract class DomainModelBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
