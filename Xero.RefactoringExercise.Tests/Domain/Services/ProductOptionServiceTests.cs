using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.Domain.Exceptions;
using Xero.RefactoringExercise.Domain.Modles;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.Tests.Support;
using Xero.RefactoringExercise.Tests.Support.Data;

namespace Xero.RefactoringExercise.Tests.Domain.Services
{
    [TestClass]
    public class ProductOptionServiceTests : UnitTestBase
    {
        private ProductOptionService _productOptionService;

        [TestInitialize]
        public void Init()
        {
            UserContext.Setup(uc => uc.IdentityName).Returns("TestJing");
            UserContextService.Setup(ucs => ucs.Current).Returns(UserContext.Object);

            _productOptionService = new ProductOptionService(Repository, UserContextService.Object);

            DbContext.Reset();

            var allProducts = new List<Product> {SampleProducts.SamsungS7Product, SampleProducts.IPhone6SProduct, SampleProducts.SamsungS6Product};
            var mockedProductsDbSet = DbSetMockingHelper.CreateMockedDbSet(allProducts.AsQueryable());
            DbContext.Setup(c => c.Set<Product>()).Returns(mockedProductsDbSet.Object);

            var allProductOptions = new List<ProductOption>
            {
                SampleProductOptions.WhiteIPhone6S,
                SampleProductOptions.RoseIPhone6S,
                SampleProductOptions.BlackIPhone6S,
                SampleProductOptions.WhiteGalaxyS7,
                SampleProductOptions.BlackGalaxyS7
            };

            var mockedProductOptionsDbSet = DbSetMockingHelper.CreateMockedDbSet(allProductOptions.AsQueryable());
            DbContext.Setup(c => c.Set<ProductOption>()).Returns(mockedProductOptionsDbSet.Object);
        }

        [TestMethod]
        public void FindAllProducOptionsReturnsCorrectResults()
        {
            var iPhone6SOptionsDomainModelResult = _productOptionService.FindAllProductOptions(SampleProducts.IPhone6SProduct.Id).ToList();
            Assert.AreEqual(3, iPhone6SOptionsDomainModelResult.Count());
            Assert.AreEqual(iPhone6SOptionsDomainModelResult[0].Name, SampleProductOptions.BlackIPhone6S.Name);
            Assert.AreEqual(iPhone6SOptionsDomainModelResult[1].Name, SampleProductOptions.RoseIPhone6S.Name);
            Assert.AreEqual(iPhone6SOptionsDomainModelResult[2].Name, SampleProductOptions.WhiteIPhone6S.Name);

            var samsungS6OptionsDomainModelResult = _productOptionService.FindAllProductOptions(SampleProducts.SamsungS6Product.Id).ToList();
            Assert.AreEqual(0, samsungS6OptionsDomainModelResult.Count());

            var dummyProductOptionsDomainModelResult = _productOptionService.FindAllProductOptions(new Guid()).ToList();
            Assert.AreEqual(0, dummyProductOptionsDomainModelResult.Count());
        }

        [TestMethod]
        public void GetProducOptionByIdReturnsCorrectResults()
        {
            var whiteIPhone6SOptionsDomainModelResult = _productOptionService.GetProductOptionById(SampleProducts.IPhone6SProduct.Id,
                SampleProductOptions.WhiteIPhone6S.Id);
            Assert.IsNotNull(whiteIPhone6SOptionsDomainModelResult);
            Assert.AreEqual(whiteIPhone6SOptionsDomainModelResult.Id, SampleProductOptions.WhiteIPhone6S.Id);
            Assert.AreEqual(whiteIPhone6SOptionsDomainModelResult.Name, SampleProductOptions.WhiteIPhone6S.Name);
        }

        [TestMethod]
        public void GetProducOptionByIdReturnsEmptyWithNonexistentIds()
        {
            var nonexistentProductIdDomainModelResult = _productOptionService.GetProductOptionById(new Guid(),
                SampleProductOptions.WhiteIPhone6S.Id);
            Assert.IsNull(nonexistentProductIdDomainModelResult);

            var nonexistentProductOptionIdDomainModelResult = _productOptionService.GetProductOptionById(SampleProducts.IPhone6SProduct.Id,
                new Guid());
            Assert.IsNull(nonexistentProductOptionIdDomainModelResult);

            var unmatchedProductAndProductOptionIdsDomainModelResult = _productOptionService.GetProductOptionById(SampleProducts.IPhone6SProduct.Id,
                SampleProductOptions.WhiteGalaxyS7.Id);
            Assert.IsNull(unmatchedProductAndProductOptionIdsDomainModelResult);
        }

        [TestMethod]
        public void CreateProductOptionSucceed()
        {
            DbContext.Setup(dbc => dbc.Set<ProductOption>().Add(It.IsAny<ProductOption>())).Verifiable();
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var newProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            _productOptionService.Create(SampleProducts.IPhone6SProduct.Id, newProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.Set<ProductOption>().Add(It.IsAny<ProductOption>()), Times.Once);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void CreateProductOptionWithNonexsitentProductShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.Set<ProductOption>().Add(It.IsAny<ProductOption>())).Verifiable();
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var newProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            _productOptionService.Create(new Guid(), newProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.Set<ProductOption>().Add(It.IsAny<ProductOption>()), Times.Never);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateProductOptionErrorShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.Set<ProductOption>().Add(It.IsAny<ProductOption>())).Verifiable();
            DbContext.Setup(dbc => dbc.SaveChanges()).Throws<InvalidOperationException>().Verifiable();

            var newProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            _productOptionService.Create(SampleProducts.IPhone6SProduct.Id, newProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.Set<ProductOption>().Add(It.IsAny<ProductOption>()), Times.Once);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateProductOptionSucceed()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var updatingProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "White to Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            var updatedProductOptionDomainModel = _productOptionService.Update(SampleProducts.IPhone6SProduct.Id,
                SampleProductOptions.WhiteIPhone6S.Id, updatingProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
            Assert.IsNotNull(updatedProductOptionDomainModel);
            Assert.AreEqual(updatingProductOptionDomainModel.Name, updatedProductOptionDomainModel.Name);
            Assert.AreEqual(updatingProductOptionDomainModel.Description, updatedProductOptionDomainModel.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateProductOptionErrorShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Throws<InvalidOperationException>().Verifiable();

            var updatingProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "White to Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            var updatedProductOptionDomainModel = _productOptionService.Update(SampleProducts.IPhone6SProduct.Id,
                SampleProductOptions.WhiteIPhone6S.Id, updatingProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void UpdateProductOptionWithNonexsitentProductShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var updatingProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "White to Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            _productOptionService.Update(new Guid(),
                SampleProductOptions.WhiteIPhone6S.Id, updatingProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void UpdateProductOptionWithNonexsitentProductOptionShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var updatingProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "White to Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            _productOptionService.Update(SampleProducts.IPhone6SProduct.Id,
                new Guid(), updatingProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void UpdateProductOptionWithUnmatchedProductAndProductOptionShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var updatingProductOptionDomainModel = new ProductOptionDomainModel
            {
                Name = "White to Yellow IPhone 6s",
                Description = "Very nice yellow iphone",
            };

            _productOptionService.Update(SampleProducts.IPhone6SProduct.Id,
                new Guid(), updatingProductOptionDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void DeleteProductOptionSucceed()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();


            _productOptionService.Delete(SampleProducts.IPhone6SProduct.Id, SampleProductOptions.WhiteIPhone6S.Id);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteProductOptionErrorShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Throws<InvalidOperationException>().Verifiable();

            _productOptionService.Delete(SampleProducts.IPhone6SProduct.Id, SampleProductOptions.WhiteIPhone6S.Id);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void DeleteProductOptionWithNonexsitentProductShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            _productOptionService.Delete(new Guid(), SampleProductOptions.WhiteIPhone6S.Id);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void DeleteProductOptionWithNonexsitentProductOptionShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            _productOptionService.Delete(SampleProducts.IPhone6SProduct.Id, new Guid());

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void DeleteProductOptionWithUnmatchedProductAndProductOptionShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            _productOptionService.Delete(SampleProducts.IPhone6SProduct.Id, new Guid());

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }
    }
}