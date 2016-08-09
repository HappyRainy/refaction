using System;
using System.Collections.Generic;
using Xero.RefactoringExercise.Domain.Modles;

namespace Xero.RefactoringExercise.Domain.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductDomainModel> FindAllProducts();

        /// <summary>
        /// Finds all products matching the specified name.
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        IEnumerable<ProductDomainModel> FindProductsByName(string productName);

        /// <summary>
        /// Gets the project that matches the specified ID 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductDomainModel GetProductById(Guid id);

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDomainModel"></param>
        /// <returns></returns>
        ProductDomainModel Create(ProductDomainModel productDomainModel);

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDomainModel"></param>
        /// <returns></returns>
        ProductDomainModel Update(Guid id, ProductDomainModel productDomainModel);

        /// <summary>
        /// Deletes a product and its options.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);
    }
}
