using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NLog;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.DAL.Supports;
using Xero.RefactoringExercise.Domain.Exceptions;
using Xero.RefactoringExercise.Domain.Modles;

namespace Xero.RefactoringExercise.Domain.Services
{
    public class ProductOptionService : IProductOptionService
    {
        private readonly IRepository _repository;
        private readonly IUserContextService _userContextService;
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public ProductOptionService(IRepository repository, IUserContextService userContextService)
        {
            _repository = repository;
            _userContextService = userContextService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<ProductOptionDomainModel> FindAllProductOptions(Guid productId)
        {
            var productOptions = _repository.GetAll<ProductOption>(pos => pos.Where(p => p.ProductId.Equals(productId)).OrderBy(p => p.Name));

            return Mapper.Map<List<ProductOptionDomainModel>>(productOptions);
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <returns></returns>
        public ProductOptionDomainModel GetProductOptionById(Guid productId, Guid productOptionId)
        {
            var productOption = _repository.GetFirst<ProductOption>( po => po.ProductId.Equals(productId) && po.Id.Equals(productOptionId));

            return Mapper.Map<ProductOptionDomainModel>(productOption);
        }

        /// <summary>
        /// Creates a new product option to the specified product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionDomainModel"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseAccessException">Thrown for unexpected Db access exception</exception>
        /// <exception cref="UniqueConstraintException">Thrown when trying to insert/update with duplicate unique value</exception>
        /// <exception cref="Exception">Thrown for unexpected exception</exception>
        public ProductOptionDomainModel Create(Guid productId, ProductOptionDomainModel productOptionDomainModel)
        {
            var parentProduct = _repository.GetById<Product>(productId);

            if(parentProduct == null) throw new RecordNotFoundException();

            var newProductOption = Mapper.Map<ProductOption>(productOptionDomainModel);

            try
            {
                newProductOption.ProductId = productId;

                _repository.Create(newProductOption, _userContextService.Current.IdentityName);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Failed to create new product option");
                SqlExceptionHandler.HandleException(ex);
                throw;
            }

            return Mapper.Map<ProductOptionDomainModel>(newProductOption);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <param name="productOptionDomainModel"></param>
        /// <returns></returns>
        /// <exception cref="ConcurrencyException">Thrown when concurrency racing condition happens</exception>
        /// <exception cref="DatabaseAccessException">Thrown for unexpected Db access exception</exception>
        /// <exception cref="UniqueConstraintException">Thrown when trying to insert/update with duplicate unique value</exception>
        /// <exception cref="Exception">Thrown for unexpected exception</exception>        
        public ProductOptionDomainModel Update(Guid productId, Guid productOptionId, ProductOptionDomainModel productOptionDomainModel)
        {
            var updatingProductOption = _repository.GetFirst<ProductOption>(po => po.ProductId.Equals(productId) && po.Id.Equals(productOptionId));
            if (updatingProductOption == null) throw new RecordNotFoundException();

            Mapper.Map(productOptionDomainModel, updatingProductOption);

            try
            {
                _repository.Update(updatingProductOption, _userContextService.Current.IdentityName);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Failed to update product option");
                SqlExceptionHandler.HandleException(ex);
                throw;
            }

            return Mapper.Map<ProductOptionDomainModel>(updatingProductOption);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productOptionId"></param>
        /// <exception cref="ConcurrencyException">Thrown when concurrency racing condition happens</exception>
        /// <exception cref="DatabaseAccessException">Thrown for unexpected Db access exception</exception>
        /// <exception cref="UniqueConstraintException">Thrown when trying to insert/update with duplicate unique value</exception>
        /// <exception cref="Exception">Thrown for unexpected exception</exception>       
        public void Delete(Guid productId, Guid productOptionId)
        {
            var deleteingProductOption = _repository.GetFirst<ProductOption>(po => po.ProductId.Equals(productId) && po.Id.Equals(productOptionId));
            if (deleteingProductOption == null) throw new RecordNotFoundException();

            try
            {
                _repository.Delete(deleteingProductOption);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Failed to delete product option");
                SqlExceptionHandler.HandleException(ex);
                throw;
            }
        }
    }
}
