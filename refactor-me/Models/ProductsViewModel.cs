using System.Collections.Generic;

namespace Xero.RefactoringExercise.WebApi.Models
{
    /// <summary>
    /// View model to be used for returning list of product options.
    /// </summary>
    public class ProductOptionsViewModel
    {
        public IEnumerable<ProductOptionViewModel> Items { get; set; }
    }
}