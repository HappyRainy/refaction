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
    public class ProductService : IProductService
    {
        private readonly IRepository _repository;
        private readonly IUserContextService _userContextService;
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public ProductService(IRepository repository, IUserContextService userContextService)
        {
            _repository = repository;
            _userContextService = userContextService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductDomainModel> FindAllProducts()
        {
            return Mapper.Map<List<ProductDomainModel>>(_repository.GetAll<Product>(products => products.OrderBy(p => p.Name)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public IEnumerable<ProductDomainModel> FindProductsByName(string productName)
        {
            return
                Mapper.Map<List<ProductDomainModel>>(_repository.Get<Product>(products => products.Name.ToLower().Contains(productName.ToLower()),
                    products => products.OrderBy(p => p.Name)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDomainModel GetProductById(Guid id)
        {
            return Mapper.Map<ProductDomainModel>(_repository.GetById<Product>(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productDomainModel"></param>
        /// <returns></returns>
        /// <exception cref="ConcurrencyException">Thrown when concurrency racing condition happens</exception>
        /// <exception cref="DatabaseAccessException">Thrown for unexpected Db access exception</exception>
        /// <exception cref="UniqueConstraintException">Thrown when trying to insert/update with duplicate unique value</exception>
        /// <exception cref="Exception">Thrown for unexpected exception</exception>        
        public ProductDomainModel Create(ProductDomainModel productDomainModel)
        {
            var newProduct = Mapper.Map<Product>(productDomainModel);

            try
            {
                _repository.Create(newProduct, _userContextService.Current.IdentityName);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Failed to create new product");
                SqlExceptionHandler.HandleException(ex);
                throw;
            }

            return Mapper.Map<ProductDomainModel>(newProduct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDomainModel"></param>
        /// <returns></returns>
        /// <exception cref="ConcurrencyException">Thrown when concurrency racing condition happens</exception>
        /// <exception cref="DatabaseAccessException">Thrown for unexpected Db access exception</exception>
        /// <exception cref="UniqueConstraintException">Thrown when trying to insert/update with duplicate unique value</exception>
        /// <exception cref="Exception">Thrown for unexpected exception</exception>        
        public ProductDomainModel Update(Guid id, ProductDomainModel productDomainModel)
        {
            var updatingProduct = _repository.GetById<Product>(id);

            if (updatingProduct == null) throw new RecordNotFoundException();

            Mapper.Map(productDomainModel, updatingProduct);

            try
            {
                _repository.Update(updatingProduct, _userContextService.Current.IdentityName);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Failed to update product");
                SqlExceptionHandler.HandleException(ex);
                throw;
            }

            return Mapper.Map<ProductDomainModel>(updatingProduct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ConcurrencyException">Thrown when concurrency racing condition happens</exception>
        /// <exception cref="DatabaseAccessException">Thrown for unexpected Db access exception</exception>
        /// <exception cref="UniqueConstraintException">Thrown when trying to insert/update with duplicate unique value</exception>
        /// <exception cref="Exception">Thrown for unexpected exception</exception>        
        public void Delete(Guid id)
        {
            var deletingProduct = _repository.GetById<Product>(id);

            if (deletingProduct == null) throw new RecordNotFoundException();

            try
            {
                //Cascade deleting ProductOptions related to this product within db
                _repository.Delete(deletingProduct);
                _repository.Save();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex, "Failed to delete product");
                SqlExceptionHandler.HandleException(ex);
                throw;
            }
        }
    }
}
