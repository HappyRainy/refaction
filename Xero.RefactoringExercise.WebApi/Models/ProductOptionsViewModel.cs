using System.Collections.Generic;

namespace Xero.RefactoringExercise.WebApi.Models
{
    /// <summary>
    /// View model to be used for returning list of products.
    /// </summary>
    public class ProductsViewModel
    {
        public IEnumerable<ProductViewModel> Items { get; set; }
    }
}