using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Xero.RefactoringExercise.Domain.Exceptions;
using Xero.RefactoringExercise.Domain.Modles;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.Tests.Support;
using Xero.RefactoringExercise.Tests.Support.Data;
using Xero.RefactoringExercise.WebApi.Controllers;
using Xero.RefactoringExercise.WebApi.Models;

namespace Xero.RefactoringExercise.Tests.WebApi.Controllers
{
    [TestClass]
    public class ProductControllerTests : ControllerTestBase
    {
        private ProductsController _productController;
        private Mock<IProductService> _productService;

        [TestInitialize]
        public void Init()
        {
            _productService = new Mock<IProductService>();
            _productController = new ProductsController(_productService.Object);
        }

        #region Product tests

        [TestMethod]
        public void ProductsGetAllReturnsCorrectResults()
        {
            var allProductDomainModels = new List<ProductDomainModel>
            {
                SampleProductDomainModels.IPhone6SProductDomainModel,
                SampleProductDomainModels.SamsungS7ProductDomainModel
            };

            _productService.Setup(s => s.FindAllProducts()).Returns(allProductDomainModels);

            var response = _productController.GetAll() as OkNegotiatedContentResult<ProductsViewModel>;

            Assert.IsNotNull(response);

            Assert.AreEqual(2, response.Content.Items.Count());

            Assert.IsTrue(response.Content.Items.All(item =>
            {
                var actualItemJson = JsonConvert.SerializeObject(item);
                return actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductViewModels.SamsungS7ProductModel)) ||
                       actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductViewModels.IPhone6SProductModel));
            }));
        }

        [TestMethod]
        public void ProductsSearchByNameReturnsCorrectResults()
        {
            var searchedDomainModels = new List<ProductDomainModel>
            {
                SampleProductDomainModels.SamsungS6ProductDomainModel,
                SampleProductDomainModels.SamsungS7ProductDomainModel
            };

            _productService.Setup(s => s.FindProductsByName("Samsung")).Returns(searchedDomainModels);

            var responseSamsung = _productController.SearchByName("Samsung") as OkNegotiatedContentResult<ProductsViewModel>;

            Assert.IsNotNull(responseSamsung);
            Assert.AreEqual(2, responseSamsung.Content.Items.Count());
            Assert.AreEqual(responseSamsung.Content.Items.ToList()[0].Name, SampleProductDomainModels.SamsungS6ProductDomainModel.Name);
            Assert.AreEqual(responseSamsung.Content.Items.ToList()[1].Name, SampleProductDomainModels.SamsungS7ProductDomainModel.Name);
        }

        [TestMethod]
        public void ProductsGetByIdReturnsCorrectResult()
        {
            var returnedDomainModel = SampleProductDomainModels.IPhone6SProductDomainModel;
            _productService.Setup(s => s.GetProductById(SampleProductDomainModels.IPhone6SProductDomainModel.Id)).Returns(returnedDomainModel);

            var responseIPhone =
                _productController.GetProduct(SampleProductDomainModels.IPhone6SProductDomainModel.Id) as OkNegotiatedContentResult<ProductViewModel>;

            Assert.IsNotNull(responseIPhone);
            Assert.AreEqual(SampleProductDomainModels.IPhone6SProductDomainModel.Name, responseIPhone.Content.Name);
        }

        [TestMethod]
        public void ProductsGetByDummyIdReturnsNotFoundResult()
        {
            var dummyId = new Guid();
            _productService.Setup(s => s.GetProductById(dummyId)).Returns((ProductDomainModel) null);

            var responseNotFound = _productController.GetProduct(dummyId) as NotFoundResult;
            Assert.IsNotNull(responseNotFound);
        }

        [TestMethod]
        public void ProductsCreateSucceed()
        {
            var newProduct = new ProductViewModel {Name = "Test Product", Description = "A test product", Price = 100.00m, DeliveryPrice = 0.00m};
            var savedProductDomainModelReturnd = Mapper.Map<ProductDomainModel>(newProduct);

            _productService.Setup(s => s.Create(It.IsAny<ProductDomainModel>())).Returns(savedProductDomainModelReturnd);

            var newProductCreatedResponse = _productController.Create(newProduct) as CreatedNegotiatedContentResult<ProductViewModel>;

            Assert.IsNotNull(newProductCreatedResponse);
        }

        [TestMethod]
        public void ProductsCreateReturns400ForInvalidModel()
        {
            var badRequestResponseForEmptyModel = _productController.Create(null);

            Assert.IsNotNull(badRequestResponseForEmptyModel as BadRequestErrorMessageResult);

            var invalidModel = new ProductViewModel {Description = "A test product", Price = 100.00m, DeliveryPrice = 0.00m};
            _productController.ValidateViewModel(invalidModel);

            var badRequestResponseForInvalidModel = _productController.Create(invalidModel);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductCreateReturns400ForDuplicatedProduct()
        {
            var newProduct = new ProductViewModel {Name = "Test Product", Description = "A test product", Price = 100.00m, DeliveryPrice = 0.00m};
            _productService.Setup(s => s.Create(It.IsAny<ProductDomainModel>()))
                .Throws(new UniqueConstraintException("Test message", new Exception()));

            var badRequestResponseForInvalidModel = _productController.Create(newProduct);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductsUpdateSucceed()
        {
            var updateProduct = new ProductViewModel
            {
                Name = "Updated Product",
                Description = "A test product",
                Price = 100.00m,
                DeliveryPrice = 0.00m
            };
            var updateProductDomainModelReturnd = Mapper.Map<ProductDomainModel>(updateProduct);
            var expectedProductViewModel = JsonConvert.SerializeObject(
                Mapper.Map<ProductViewModel>(updateProductDomainModelReturnd)
                );

            _productService.Setup(s => s.Update(It.IsAny<Guid>(), (It.IsAny<ProductDomainModel>()))).Returns(updateProductDomainModelReturnd);

            var productUpdatedResponse =
                _productController.Update(SampleProducts.IPhone6SProduct.Id, updateProduct) as OkNegotiatedContentResult<ProductViewModel>;

            Assert.IsNotNull(productUpdatedResponse);
            Assert.AreEqual(expectedProductViewModel, JsonConvert.SerializeObject(productUpdatedResponse.Content));
        }

        [TestMethod]
        public void ProductsUpdateReturns400ForInvalidModel()
        {
            var badRequestResponseForEmptyModel = _productController.Update(new Guid(), null);

            Assert.IsNotNull(badRequestResponseForEmptyModel as BadRequestErrorMessageResult);

            var invalidModel = new ProductViewModel {Description = "A test product", Price = 100.00m, DeliveryPrice = 0.00m};
            _productController.ValidateViewModel(invalidModel);

            var badRequestResponseForInvalidModel = _productController.Update(SampleProducts.IPhone6SProduct.Id, invalidModel);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductsUpdateReturns400ForNonexistenceProduct()
        {
            _productService.Setup(s => s.Update(It.IsAny<Guid>(), (It.IsAny<ProductDomainModel>()))).Throws(new RecordNotFoundException());

            var updateProduct = new ProductViewModel
            {
                Name = "Updated Product",
                Description = "A test product",
                Price = 100.00m,
                DeliveryPrice = 0.00m
            };
            var badRequestResponse = _productController.Update(new Guid(), updateProduct);

            Assert.IsNotNull(badRequestResponse as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductUpdateReturns400ForDuplicatedProduct()
        {
            _productService.Setup(s => s.Update(It.IsAny<Guid>(), It.IsAny<ProductDomainModel>()))
                .Throws(new UniqueConstraintException("Test message", new Exception()));

            var updateProduct = new ProductViewModel {Name = "Test Product", Description = "A test product", Price = 100.00m, DeliveryPrice = 0.00m};

            var badRequestResponseForInvalidModel = _productController.Update(new Guid(), updateProduct);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductsDeleteSucceed()
        {
            _productService.Setup(s => s.Delete(It.IsAny<Guid>())).Verifiable();

            var productDeletedResponse = _productController.Delete(SampleProducts.IPhone6SProduct.Id) as OkResult;

            Assert.IsNotNull(productDeletedResponse);
        }

        [TestMethod]
        public void ProductsDeleteReturns400ForNonexistenceProduct()
        {
            _productService.Setup(s => s.Delete(It.IsAny<Guid>())).Throws(new RecordNotFoundException());

            var badRequestResponse = _productController.Delete(new Guid());

            Assert.IsNotNull(badRequestResponse as BadRequestErrorMessageResult);
        }

        #endregion
    }
}
