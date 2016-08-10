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
    /// Actions for product options
    /// </summary>
    [RoutePrefix("products/{productId:guid}/options")]
    public class ProductOptionsController : ControllerBase
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly IProductOptionService _productOptionService;

        public ProductOptionsController(IProductOptionService productOptionService)
        {
            _productOptionService = productOptionService;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            var productOptionsViewModel = new ProductOptionsViewModel
            {
                Items = Mapper.Map<List<ProductOptionViewModel>>(_productOptionService.FindAllProductOptions(productId).ToList())
            };

            return Ok(productOptionsViewModel);
        }

        [Route("{id:guid}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            var option = _productOptionService.GetProductOptionById(productId, id);

            if (option != null) return Ok(Mapper.Map<ProductOptionViewModel>(option));

            _log.Warn($"Product not found with Id {id}");

            return NotFound();
        }

        [Route]
        [HttpPost]
        [AuthenticatedOnly]
        public IHttpActionResult CreateOption(Guid productId, ProductOptionViewModel productOption)
        {
            if (productOption == null || !ModelState.IsValid) return BadRequest("Product option model is invalid.");

            try
            {
                var createdProductOptionDomainModel = _productOptionService.Create(productId, Mapper.Map<ProductOptionDomainModel>(productOption));

                return Created(string.Empty, Mapper.Map<ProductOptionViewModel>(createdProductOptionDomainModel));
            }
            catch (RecordNotFoundException)
            {
                return BadRequest("Specidied product not found.");
            }
            catch (UniqueConstraintException)
            {
                return BadRequest("Product option already exists, please check your data.");
            }
        }

        [Route("{id:guid}")]
        [HttpPut]
        [AuthenticatedOnly]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, ProductOptionViewModel productOption)
        {
            if (productOption == null || !ModelState.IsValid) return BadRequest("Product option model is invalid.");

            try
            {
                var updatedProductOptionDomainModle = _productOptionService.Update(productId, id, Mapper.Map<ProductOptionDomainModel>(productOption));

                return Ok(Mapper.Map<ProductOptionViewModel>(updatedProductOptionDomainModle));
            }
            catch (RecordNotFoundException)
            {
                return BadRequest("The product option you are updating does not exis, please check your product and product option ids.");
            }
            catch (UniqueConstraintException)
            {
                return BadRequest("There is already a product option has the same info, please check your data.");
            }
        }

        [Route("{id:guid}")]
        [HttpDelete]
        [AuthenticatedOnly]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            try
            {
                _productOptionService.Delete(productId, id);
                return Ok();
            }
            catch (RecordNotFoundException)
            {
                return BadRequest("The product option you are deleting does not exist, please check your product and product option ids");
            }
        }
    }
}
