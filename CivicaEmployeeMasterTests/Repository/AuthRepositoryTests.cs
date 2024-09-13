using AutoFixture;
using CivicaEmployeeMaster.Data.Implementation;
using CivicaEmployeeMaster.Data;
using CivicaEmployeeMaster.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivicaEmployeeMasterTests.Repository
{
    public class AuthRepositoryTests
    {
        [Fact]
        public void RegisterUser_ReturnTrue()
        {
            //Arrange
            var fixture = new Fixture();
            var user = fixture.Create<User>();
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            mockAbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.RegisterUser(user);
            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Add(user), Times.Once);
            mockAbContext.Verify(c => c.SaveChanges(), Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);

        }
        [Fact]
        public void RegisterUser_ReturnFalse()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.RegisterUser(null);
            //Assert
            Assert.False(actual);
        }
        [Fact]
        public void ValidateUser_ReturnTrue()
        {
            //Arrange
            var users = new List<User>
            {
                new User
            {
                UserId = 1,
                Name = "firstname",
                Email = "email@example.com",
                LoginId = "loginid"

                },
                new User
            {
                UserId = 2,
                Name = "firstname",
                Email = "email@example.com",
                LoginId = "loginid1"

                },
            }.AsQueryable();
            var username = "loginid";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.ValidateUser(username);
            //Assert
            Assert.NotNull(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void ValidateUser_WhenUsersIsNull()
        {
            //Arrange
            var users = new List<User>().AsQueryable();
            var username = "loginid";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.ValidateUser(username);
            //Assert
            Assert.Null(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void UserExist_WhenUsersIsNull()
        {
            //Arrange
            var users = new List<User>().AsQueryable();
            var loginId = "loginid";
            var email = "abc@gmail.com";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.UserExist(loginId, email);
            //Assert
            Assert.False(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsTrue()
        {
            //Arrange
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAppDbContext = new Mock<IAppDbContext>();
            mockAppDbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            mockAppDbContext.Setup(c => c.SaveChanges()).Returns(1);
            var target = new AuthRepository(mockAppDbContext.Object);

            var fixture = new Fixture();
            var user = fixture.Create<User>();


            //Act
            var actual = target.UpdateUser(user);

            //Assert
            Assert.True(actual);
            mockDbSet.Verify(c => c.Update(user), Times.Once);
            mockAppDbContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Fact]
        public void UpdateUser_ReturnsFalse()
        {
            //Arrange
            User user = null;
            var mockAbContext = new Mock<IAppDbContext>();
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.UpdateUser(user);
            //Assert
            Assert.False(actual);
        }
        [Fact]
        public void UserExist_WhenUsersIsThere()
        {
            //Arrange
            var users = new List<User>
            {
                new User
            {
                UserId = 1,
                Name = "firstname",
                Email = "email@example.com",
                LoginId = "loginid",

                },
                new User
            {
                UserId= 2,
                Name = "firstname",
                Email = "email@example.com",
                LoginId = "loginid1",

                },
            }.AsQueryable();
            var loginId = "loginid";
            var email = "abc@gmail.com";
            var mockDbSet = new Mock<DbSet<User>>();
            var mockAbContext = new Mock<IAppDbContext>();
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(c => c.Expression).Returns(users.Expression);
            mockAbContext.SetupGet(c => c.Users).Returns(mockDbSet.Object);
            var target = new AuthRepository(mockAbContext.Object);

            //Act
            var actual = target.UserExist(loginId, email);
            //Assert
            Assert.True(actual);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Provider, Times.Once);
            mockDbSet.As<IQueryable<User>>().Verify(c => c.Expression, Times.Once);
            mockAbContext.VerifyGet(c => c.Users, Times.Once);
        }
    }
}
