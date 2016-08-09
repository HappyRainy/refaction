using System.Collections.Generic;

namespace Xero.RefactoringExercise.Domain.Modles
{
    public class ProductDomainModel : DomainModelBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        public List<ProductOptionDomainModel> ProductOptions { get; set; }
    }
}
