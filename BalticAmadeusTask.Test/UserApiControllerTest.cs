using AutoMapper;
using BalticAmadeusTask.Controllers;
using BalticAmadeusTask.Data;
using BalticAmadeusTask.Helpers;
using BalticAmadeusTask.Models;
using BalticAmadeusTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BalticAmadeusTask.Test
{
    public class UserApiControllerTest
    {
        private readonly UserApiController _apiController;

        private readonly Guid _knownGuid1 = new Guid("53f67d5a-a7c6-4ff3-2f96-08d6a97c97ef");
        private readonly Guid _knownGuid2 = new Guid("099886af-af84-4a96-2f97-08d6a97c97ef");

        public UserApiControllerTest()
        {
            var options = new DbContextOptionsBuilder<BalticAmadeusTaskContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<RegisteredUser, RegisteredUserD>(); cfg.CreateMap<RegisteredUserD, RegisteredUser>(); });
            var mapper = new Mapper(config);

            var context = new BalticAmadeusTaskContext(options);
            _apiController = new UserApiController(context,mapper, new PasswordPolicyService(), new HashingService());

            Mock<IPasswordPolicyService> mockPasswordPolicy = new Mock<IPasswordPolicyService>(MockBehavior.Strict);
            mockPasswordPolicy.Setup(x => x.PasswordErrors(It.IsAny<string>()))
                .Returns((string x) => null );

            Mock<IHashingService> mockHashing = new Mock<IHashingService>(MockBehavior.Strict);
            mockHashing.Setup(x => x.GenerateHashString(It.IsAny<string>()))
                .Returns((string x) => x);

            context.Add(new RegisteredUserD()
            {
                Id = _knownGuid1,
                Name = "111"
            });
            context.Add(new RegisteredUserD()
            {
                Id = _knownGuid2,
                Name = "222"
            });
            context.SaveChanges();
        }

        [Fact]
        public void WhenGetAllCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _apiController.GetUserModel();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void WhenGetAllCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _apiController.GetUserModel().Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var items = Assert.IsType<List<RegisteredUser>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        //GetAll with filtering
        [Fact]
        public void WhenGetAllWithFilteringCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _apiController.GetUserModel(new UserFiltering()).Result;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void WhenGetAllFilteringCalled_ReturnsCorrectNumber1()
        {

            // Act
            var okResult = _apiController.GetUserModel(new UserFiltering()).Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var items = Assert.IsType<List<RegisteredUser>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public void WhenGetAllFilteringCalled_ReturnsCorrectNumber2()
        {
            // Act
            var okResult = _apiController.GetUserModel(new UserFiltering(){Name = "1"}).Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var items = Assert.IsType<List<RegisteredUser>>(okResult.Value);
            Assert.Single( items);
        }

        [Fact]
        public void GetFilteringCalled_ReturnsCorrectNumber3()
        {
            // Act
            var okResult = _apiController.GetUserModel(new UserFiltering() { Name = "2" }).Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var items = Assert.IsType<List<RegisteredUser>>(okResult.Value);
            Assert.Single(items);
        }

        [Fact]
        public void GetFilteringCalled_ReturnsCorrectNumber4()
        {
            // Act
            var okResult = _apiController.GetUserModel(new UserFiltering() { Name = "WXYZ" }).Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            var items = Assert.IsType<List<RegisteredUser>>(okResult.Value);
            Assert.Empty(items);
        }

        //Get by Id
        [Fact]
        public void GetByIdUnknownGuidPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _apiController.GetUserModel(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetByIdExistingGuidPassed_ReturnsOkResult()
        {
            // Arrange
            var testGuid = _knownGuid1;

            // Act
            var okResult = _apiController.GetUserModel(testGuid);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetByIdExistingGuidPassed_ReturnsRightItem()
        {
            // Arrange
            var testGuid = _knownGuid2;

            // Act
            var okResult = _apiController.GetUserModel(testGuid).Result as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.IsType<RegisteredUser>(okResult.Value);
            Assert.Equal(testGuid, (okResult.Value as RegisteredUser).Id);
        }


        //Creation
        [Fact]
        public void AddInvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var nameMissingItem = new RegisteredUser()
            {
            };
            _apiController.ModelState.AddModelError("Name", "Required");

            // Act
            var badResponse = _apiController.PostUserModel(nameMissingItem).Result;

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void AddValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            RegisteredUser testItem = new RegisteredUser()
            {
                Name = RandomGeneratorHelper.RandomAlpha(20),
                Email = RandomGeneratorHelper.RandomEmail(),
                DateOfBirth = DateTime.Now,
                Password = RandomGeneratorHelper.RandomAlphaNum(20),
                AdditionalInfo = RandomGeneratorHelper.RandomAlphaNum(20),
            };

            // Act
            var createdResponse = _apiController.PostUserModel(testItem).Result;

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void AddValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            RegisteredUser testItem = new RegisteredUser()
            {
                Name = RandomGeneratorHelper.RandomAlpha(20),
                Email = RandomGeneratorHelper.RandomEmail(),
                DateOfBirth = DateTime.Now,
                Password = RandomGeneratorHelper.RandomAlphaNum(20),
                AdditionalInfo = RandomGeneratorHelper.RandomAlphaNum(20),
            };

            // Act
            var createdResponse = _apiController.PostUserModel(testItem).Result as CreatedAtActionResult;
            var item = createdResponse.Value as RegisteredUser;

            // Assert
            Assert.IsType<RegisteredUser>(item);
            Assert.Equal(testItem.Name, item.Name);
            Assert.Equal(testItem.Email, item.Email);
            Assert.Equal(testItem.AdditionalInfo, item.AdditionalInfo);
            Assert.Equal(testItem.DateOfBirth, item.DateOfBirth);
        }
    }
}
