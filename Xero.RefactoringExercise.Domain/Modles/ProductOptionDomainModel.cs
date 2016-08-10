using System;

namespace Xero.RefactoringExercise.Domain.Modles
{
    /// <summary>
    /// ProductOption domain model to be returned to upper layer
    /// </summary>
    public class ProductOptionDomainModel: DomainModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
