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
    public class ProductServiceTests : UnitTestBase
    {
        private ProductService _productService;

        [TestInitialize]
        public void Init()
        {
            UserContext.Setup(uc => uc.IdentityName).Returns("TestJing");
            UserContextService.Setup(ucs => ucs.Current).Returns(UserContext.Object);

            _productService = new ProductService(Repository, UserContextService.Object);

            DbContext.Reset();

            var allProducts = new List<Product> { SampleProducts.SamsungS7Product, SampleProducts.IPhone6SProduct, SampleProducts.SamsungS6Product };
            var mockedProductsDbSet = DbSetMockingHelper.CreateMockedDbSet(allProducts.AsQueryable());
            DbContext.Setup(c => c.Set<Product>()).Returns(mockedProductsDbSet.Object);

        }

        [TestMethod]
        public void FindAllProductsReturnsCorrectResults()
        {
            //Order is set on name by default for FindAllProducts method.
            var productsDomainModelResult = _productService.FindAllProducts().ToList();

            Assert.AreEqual(3, productsDomainModelResult.Count());
            Assert.AreEqual(productsDomainModelResult[0].Name, SampleProducts.IPhone6SProduct.Name);
            Assert.AreEqual(productsDomainModelResult[1].Name, SampleProducts.SamsungS6Product.Name);
            Assert.AreEqual(productsDomainModelResult[2].Name, SampleProducts.SamsungS7Product.Name);
        }

        [TestMethod]
        public void FindProductsByNameReturnsCorrectResults()
        {
            var productsDomainModelResult = _productService.FindProductsByName("Samsung").ToList();

            Assert.AreEqual(2, productsDomainModelResult.Count());
            Assert.AreEqual(productsDomainModelResult[0].Name, SampleProducts.SamsungS6Product.Name);
            Assert.AreEqual(productsDomainModelResult[1].Name, SampleProducts.SamsungS7Product.Name);

            var emptySearchResult = _productService.FindProductsByName("DummyName").ToList();
            Assert.AreEqual(0, emptySearchResult.Count());
        }

        [TestMethod]
        public void GetProductByIdReturnsCorrectResults()
        {
            var productDomainModelReturned = _productService.GetProductById(SampleProducts.SamsungS6Product.Id);

            Assert.IsNotNull(productDomainModelReturned);
            Assert.AreEqual(SampleProducts.SamsungS6Product.Id, productDomainModelReturned.Id);

            var emptyDomainModelReturned = _productService.GetProductById(new Guid());

            Assert.IsNull(emptyDomainModelReturned);
        }

        [TestMethod]
        public void CreateProductSucceed()
        {
            DbContext.Setup(dbc => dbc.Set<Product>().Add(It.IsAny<Product>())).Verifiable();
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var newProductDomainModel = new ProductDomainModel
            {
                Name = "Test Product 1",
                Description = "Test product 1 from jing",
                Price = 1200.00m,
                DeliveryPrice = 15.00m
            };

            _productService.Create(newProductDomainModel);

            DbContext.Verify(dbc => dbc.Set<Product>().Add(It.IsAny<Product>()), Times.Once);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateProductErrorShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.Set<Product>().Add(It.IsAny<Product>())).Verifiable();
            DbContext.Setup(dbc => dbc.SaveChanges()).Throws<InvalidOperationException>().Verifiable();

            var newProductDomainModel = new ProductDomainModel
            {
                Name = "Test Product 1",
                Description = "Test product 1 from jing",
                Price = 1200.00m,
                DeliveryPrice = 15.00m
            };

            _productService.Create(newProductDomainModel);

            DbContext.Verify(dbc => dbc.Set<Product>().Add(It.IsAny<Product>()), Times.Once);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateProductSucceed()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var updatingProductDomainModel = new ProductDomainModel
            {
                Name = "Jing Product",
                Description = "Jing Product for updating",
                Price = 1200.00m,
                DeliveryPrice = 15.00m
            };

            _productService.Update(SampleProducts.SamsungS7Product.Id, updatingProductDomainModel);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);

        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void UpdateNonexistentProductShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            var updatingProductDomainModel = new ProductDomainModel
            {
                Name = "Jing Product",
                Description = "Jing Product for updating",
                Price = 1200.00m,
                DeliveryPrice = 15.00m
            };

            _productService.Update(new Guid(), updatingProductDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateProductErrorShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Throws<InvalidOperationException>().Verifiable();

            var updatingProductDomainModel = new ProductDomainModel
            {
                Name = "Jing Product",
                Description = "Jing Product for updating",
                Price = 1200.00m,
                DeliveryPrice = 15.00m
            };

            _productService.Update(SampleProducts.SamsungS7Product.Id, updatingProductDomainModel);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void DeleteProductSucceed()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            _productService.Delete(SampleProducts.SamsungS7Product.Id);
            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(RecordNotFoundException))]
        public void DeleteNonexistentProductShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Verifiable();

            _productService.Delete(new Guid());

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteProductErrorShouldThrowException()
        {
            DbContext.Setup(dbc => dbc.SaveChanges()).Throws<InvalidOperationException>().Verifiable();

            _productService.Delete(SampleProducts.SamsungS7Product.Id);

            DbContext.Verify(dbc => dbc.SaveChanges(), Times.Once);
        }
    }
}
