using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using NLog;
using Xero.RefactoringExercise.Domain.Exceptions;
using Xero.RefactoringExercise.Domain.Modles;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.WebApi.Controllers.Support;
using Xero.RefactoringExercise.WebApi.Infrastructure.Filters;
using Xero.RefactoringExercise.WebApi.Models;

namespace Xero.RefactoringExercise.WebApi.Controllers
{
    /// <summary>
    /// Actions for products
    /// </summary>
    [RoutePrefix("products")]
    public class ProductsController : ControllerBase
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var productsViewModel = new ProductsViewModel
            {
                Items = Mapper.Map<List<ProductViewModel>>(_productService.FindAllProducts().ToList())
            };

            return Ok(productsViewModel);
        }

        [Route]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            var productsViewModel = new ProductsViewModel
            {
                Items = Mapper.Map<List<ProductViewModel>>(_productService.FindProductsByName(name).ToList())
            };

            return Ok(productsViewModel);
        }

        [Route("{id:guid}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            var productDomainModel = _productService.GetProductById(id);

            if (productDomainModel != null) return Ok(Mapper.Map<ProductViewModel>(productDomainModel));

            _log.Warn($"Product not found with Id {id}");

            return NotFound();
        }

        [Route]
        [HttpPost]
        [AuthenticatedOnly]
        public IHttpActionResult Create(ProductViewModel product)
        {
            if (product == null || !ModelState.IsValid) return BadRequest("Product model is invalid.");

            try
            {
                var createdProductDomainModle = _productService.Create(Mapper.Map<ProductDomainModel>(product));

                return Created(string.Empty, Mapper.Map<ProductViewModel>(createdProductDomainModle));
            }
            catch (UniqueConstraintException)
            {
                return BadRequest("Product already exists, please check your data.");
            }
        }

        [Route("{id:guid}")]
        [HttpPut]
        [AuthenticatedOnly]
        public IHttpActionResult Update(Guid id, ProductViewModel product)
        {
            if (product == null || !ModelState.IsValid) return BadRequest("Product model is invalid.");

            try
            {
                var updatedProductDomainModle = _productService.Update(id, Mapper.Map<ProductDomainModel>(product));

                return Ok(Mapper.Map<ProductViewModel>(updatedProductDomainModle));
            }
            catch (RecordNotFoundException)
            {
                return BadRequest("The product you are updating does not exist.");
            }
            catch (UniqueConstraintException)
            {
                return BadRequest("There is already a product has the same info, please check your data.");
            }
        }

        [Route("{id:guid}")]
        [HttpDelete]
        [AuthenticatedOnly]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                _productService.Delete(id);
                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return BadRequest("The product you are deleting does not exist.");
            }
        }
    }
}
