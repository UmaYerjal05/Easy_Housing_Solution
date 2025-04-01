using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyHousingSolutionProjectAPI.Models;
using EasyHousingSolutionProjectAPI.Repository;
using EasyHousingSolutionProjectAPI.Interface;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace EasyHousingSolutionProjectAPI.Tests
{
    [TestClass]
    public class BuyerRepositoryTests
    {
        private Mock<EasyHousingSolutionProjectDbContext> _mockContext;
        private Mock<IPropertyRepository> _mockPropertyRepo;
        private Mock<IWishlistRepository> _mockWishlistRepo;
        private BuyerRepository _buyerRepository;

        public BuyerRepositoryTests()
        {
            _mockContext = new Mock<EasyHousingSolutionProjectDbContext>();
            _mockPropertyRepo = new Mock<IPropertyRepository>();
            _mockWishlistRepo = new Mock<IWishlistRepository>();
            _buyerRepository = new BuyerRepository(_mockContext.Object, _mockPropertyRepo.Object, _mockWishlistRepo.Object);
        }

        [TestMethod]
        public void AddBuyer_ShouldReturnFalse_WhenBuyerExists()
        {
            // Arrange
            var buyer = new Buyer { BuyerId = 1, FirstName = "John", LastName = "Doe" };

            // Mock DbSet
            var mockDbSet = new Mock<DbSet<Buyer>>();
            _mockContext.Setup(m => m.Buyers).Returns(mockDbSet.Object);
            mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns(buyer);

            // Act
            var result = _buyerRepository.Add(buyer);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddBuyer_ShouldReturnTrue_WhenBuyerDoesNotExist()
        {
            // Arrange
            var buyer = new Buyer { BuyerId = 1, FirstName = "John", LastName = "Doe" };

            // Mock DbSet
            var mockDbSet = new Mock<DbSet<Buyer>>();
            _mockContext.Setup(m => m.Buyers).Returns(mockDbSet.Object);
            mockDbSet.Setup(m => m.Find(It.IsAny<int>())).Returns((Buyer)null);

            // Act
            var result = _buyerRepository.Add(buyer);

            // Assert
            Assert.IsTrue(result);
        }

 

        [TestMethod]
        public async Task AddToWishList_ShouldCallAddToWishlistMethod()
        {
            // Arrange
            var buyerId = 1;
            var propertyId = 1;

            _mockWishlistRepo.Setup(w => w.AddToWishlist(buyerId, propertyId))
                             .ReturnsAsync(true);

            // Act
            var result = await _buyerRepository.AddToWishList(buyerId, propertyId);

            // Assert
            Assert.IsTrue(result);
            _mockWishlistRepo.Verify(w => w.AddToWishlist(buyerId, propertyId), Times.Once);
        }
    }
}
