using System;

namespace Xero.RefactoringExercise.Domain.Modles
{
    public abstract class DomainModelBase
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
