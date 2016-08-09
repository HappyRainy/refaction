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
    public class ProductOptionControllerTests : ControllerTestBase
    {
        private ProductOptionsController _productOptionsController;
        private Mock<IProductOptionService> _productOptionService;

        [TestInitialize]
        public void Init()
        {
            _productOptionService = new Mock<IProductOptionService>();
            _productOptionsController = new ProductOptionsController( _productOptionService.Object);
        }


        #region Product Option
        [TestMethod]
        public void ProductOptionsGetAllReturnsCorrectResults()
        {
            var allProductOptionsDomainModels = new List<ProductOptionDomainModel>
            {
                SampleProductOptionDomainModels.BlackIPhone6SOptionDomainModel,
                SampleProductOptionDomainModels.RoseIPhone6SOptionDomainModel,
                SampleProductOptionDomainModels.WhiteIPhone6SOptionDomainModel
            };

            _productOptionService.Setup(s => s.FindAllProductOptions(It.IsAny<Guid>())).Returns(allProductOptionsDomainModels);

            var response = _productOptionsController.GetOptions(SampleProducts.IPhone6SProduct.Id) as OkNegotiatedContentResult<ProductOptionsViewModel>;

            Assert.IsNotNull(response);

            Assert.AreEqual(3, response.Content.Items.Count());

            Assert.IsTrue(response.Content.Items.All(item =>
            {
                var actualItemJson = JsonConvert.SerializeObject(item);
                return actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductOptionViewModels.BlackGalaxyS7OptionViewModel)) ||
                       actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductOptionViewModels.BlackIPhone6SOptionViewModel)) ||
                       actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductOptionViewModels.RoseIPhone6SOptionViewModel)) ||
                       actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductOptionViewModels.WhiteGalaxyS7OptionViewModel)) ||
                       actualItemJson.Equals(JsonConvert.SerializeObject(SampleProductOptionViewModels.WhiteIPhone6SOptionViewModel));
            }));
        }


        [TestMethod]
        public void ProductOptionsGetByIdReturnsCorrectResult()
        {
            _productOptionService.Setup(s => s.GetProductOptionById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(SampleProductOptionDomainModels.RoseIPhone6SOptionDomainModel);

            var responseIPhoneOption =
                _productOptionsController.GetOption(SampleProducts.IPhone6SProduct.Id, SampleProductOptions.RoseIPhone6S.Id) as OkNegotiatedContentResult<ProductOptionViewModel>;

            Assert.IsNotNull(responseIPhoneOption);
            Assert.AreEqual(SampleProductOptionDomainModels.RoseIPhone6SOptionDomainModel.Name, responseIPhoneOption.Content.Name);
            Assert.AreEqual(SampleProductOptionDomainModels.RoseIPhone6SOptionDomainModel.Id, responseIPhoneOption.Content.Id);
        }

        [TestMethod]
        public void ProductOptionsGetByDummyIdReturnsNotFoundResult()
        {
            var dummyId = new Guid();
            _productOptionService.Setup(s => s.GetProductOptionById(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns((ProductOptionDomainModel)null);

            var responseNotFound = _productOptionsController.GetOption(dummyId, dummyId) as NotFoundResult;
            Assert.IsNotNull(responseNotFound);
        }

        [TestMethod]
        public void ProductsCreateSucceed()
        {
            var newProductOption = new ProductOptionViewModel { Name = "Test Product Option", Description = "A test product Option" };
            var savedProductOptionDomainModelReturnd = Mapper.Map<ProductOptionDomainModel>(newProductOption);

            _productOptionService.Setup(s => s.Create(It.IsAny<Guid>(), It.IsAny<ProductOptionDomainModel>())).Returns(savedProductOptionDomainModelReturnd);

            var newProductOptionCreatedResponse =
                _productOptionsController.CreateOption(SampleProducts.IPhone6SProduct.Id, newProductOption) as
                    CreatedNegotiatedContentResult<ProductOptionViewModel>;

            Assert.IsNotNull(newProductOptionCreatedResponse);
        }

        [TestMethod]
        public void ProductOptionsCreateReturnsBadRequestForNonexistentProduct()
        {
            var newProductOption = new ProductOptionViewModel { Name = "Test Product Option", Description = "A test product Option" };
            _productOptionService.Setup(s => s.Create(It.IsAny<Guid>(), It.IsAny<ProductOptionDomainModel>())).Throws(new RecordNotFoundException());

            var notFoundResponse = _productOptionsController.CreateOption(new Guid(), newProductOption) as BadRequestErrorMessageResult;

            Assert.IsNotNull(notFoundResponse);
        }

        [TestMethod]
        public void ProductOptionsCreateReturns400ForInvalidModel()
        {
            var badRequestResponseForEmptyModel = _productOptionsController.CreateOption(new Guid(), null);

            Assert.IsNotNull(badRequestResponseForEmptyModel as BadRequestErrorMessageResult);

            var invalidModel = new ProductOptionViewModel { Description = "A test product option"};
            _productOptionsController.ValidateViewModel(invalidModel);

            var badRequestResponseForInvalidModel = _productOptionsController.CreateOption(new Guid(), invalidModel);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductCreateReturns400ForDuplicatedProduct()
        {
            var newProductOption = new ProductOptionViewModel { Name = "Test Product option", Description = "A test product option" };
            _productOptionService.Setup(s => s.Create(It.IsAny<Guid>(), It.IsAny<ProductOptionDomainModel>()))
                .Throws(new UniqueConstraintException("Test message", new Exception()));

            var badRequestResponseForInvalidModel = _productOptionsController.CreateOption(new Guid(), newProductOption);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductOptionsUpdateSucceed()
        {
            var updateProductOption = new ProductOptionViewModel
            {
                Name = "Updated Product Option",
                Description = "A test product option",
            };
            var updateProductOptionDomainModelReturnd = Mapper.Map<ProductOptionDomainModel>(updateProductOption);
            var expectedProductOptionViewModel = JsonConvert.SerializeObject(
                Mapper.Map<ProductOptionViewModel>(updateProductOptionDomainModelReturnd)
                );

            _productOptionService.Setup(s => s.Update(It.IsAny<Guid>(), It.IsAny<Guid>(), (It.IsAny<ProductOptionDomainModel>()))).Returns(updateProductOptionDomainModelReturnd);

            var productOptionUpdatedResponse =
                _productOptionsController.UpdateOption(SampleProducts.IPhone6SProduct.Id, SampleProductOptions.RoseIPhone6S.Id, updateProductOption) as
                    OkNegotiatedContentResult<ProductOptionViewModel>;

            Assert.IsNotNull(productOptionUpdatedResponse);
            Assert.AreEqual(expectedProductOptionViewModel, JsonConvert.SerializeObject(productOptionUpdatedResponse.Content));
        }

        [TestMethod]
        public void ProductOptionsUpdateReturns400ForInvalidModel()
        {
            var badRequestResponseForEmptyModel = _productOptionsController.UpdateOption(new Guid(), new Guid(), null);

            Assert.IsNotNull(badRequestResponseForEmptyModel as BadRequestErrorMessageResult);

            var invalidModel = new ProductOptionViewModel { Description = "A test product"};
            _productOptionsController.ValidateViewModel(invalidModel);

            var badRequestResponseForInvalidModel = _productOptionsController.UpdateOption(SampleProducts.IPhone6SProduct.Id,
                SampleProductOptions.RoseIPhone6S.Id, invalidModel);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductOptionsUpdateReturns400ForNonexistenceProduct()
        {
            _productOptionService.Setup(s => s.Update(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductOptionDomainModel>())).Throws(new RecordNotFoundException());

            var updateProductOption = new ProductOptionViewModel
            {
                Name = "Updated Product Option",
                Description = "A test product option",
            };

            var badRequestResponse = _productOptionsController.UpdateOption(new Guid(), new Guid(), updateProductOption);

            Assert.IsNotNull(badRequestResponse as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductOptionsUpdateReturns400ForDuplicatedProduct()
        {
            _productOptionService.Setup(s => s.Update(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<ProductOptionDomainModel>()))
                .Throws(new UniqueConstraintException("Test message", new Exception()));

            var updateProductOption = new ProductOptionViewModel
            {
                Name = "Updated Product Option",
                Description = "A test product option",
            };

            var badRequestResponseForInvalidModel = _productOptionsController.UpdateOption(new Guid(), new Guid(), updateProductOption);

            Assert.IsNotNull(badRequestResponseForInvalidModel as BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void ProductOptionsDeleteSucceed()
        {
            _productOptionService.Setup(s => s.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Verifiable();

            var productOptionDeletedResponse = _productOptionsController.DeleteOption(new Guid(), new Guid()) as OkResult;

            Assert.IsNotNull(productOptionDeletedResponse);
        }

        [TestMethod]
        public void ProductOptionsDeleteReturns400ForNonexistentProductOption()
        {
            _productOptionService.Setup(s => s.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new RecordNotFoundException());

            var badRequestResponse = _productOptionsController.DeleteOption(new Guid(), new Guid());

            Assert.IsNotNull(badRequestResponse as BadRequestErrorMessageResult);
        }
        #endregion
    }
}
