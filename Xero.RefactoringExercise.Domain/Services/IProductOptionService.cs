using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.RefactoringExercise.Domain.Modles;

namespace Xero.RefactoringExercise.Domain.Services
{
    public interface IProductOptionService
    {
        /// <summary>
        /// Finds all product options belong to a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IEnumerable<ProductOptionDomainModel> FindAllProductOptions(Guid productId);

        /// <summary>
        /// Finds product option by it's Id under a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <returns></returns>
        ProductOptionDomainModel GetProductOptionById(Guid productId, Guid productOptionId);

        /// <summary>
        /// Creates a new product option to the specified product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionDomainModel"></param>
        /// <returns></returns>
        ProductOptionDomainModel Create(Guid productId, ProductOptionDomainModel productOptionDomainModel);

        /// <summary>
        /// Updates the specified product option of a product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <param name="productOptionDomainModel"></param>
        /// <returns></returns>
        ProductOptionDomainModel Update(Guid productId, Guid productOptionId, ProductOptionDomainModel productOptionDomainModel);

        /// <summary>
        /// Deletes the specified product option of a product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        void Delete(Guid productId, Guid productOptionId);
    }
}
