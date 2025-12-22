using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectHub.Application.Dtos;
using ProjectHub.Domin.Entites;
using ProjectHub.Infrastructure.Data;
using ProjectHub.Infrastructure.Services;
using ProjectHub.Infrastructure.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
    

namespace ProjectHub.Tests
{
    public class UserServiceTest    
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                     .Options;
            return new AppDbContext(options);
        }

        private IOptions<JWTSettings> CreateJwtOptions()
        {
            var settings = new JWTSettings
            {
                Secret = "THIS_IS_A_TEST_SECRET_KEY_123456",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExireHours = 1
            };

            return Options.Create(settings);
        }

        [Fact]
        public async Task RegisterAsync_WhenUserDoesNotExist_ShouldReturnTrue()
        {
            // Arrange
            var context = CreateDbContext();
            var jwtOptions = CreateJwtOptions();
            var service = new UserService(context, jwtOptions);

            // Act
            var result = await service.RegisterAsync(
                userName: "testuser",
                email: "test@test.com",
                password: "123456"
            );

            // Assert
            result.Should().BeTrue();
            context.Users.Count().Should().Be(1);
        }
        [Fact]
        public async Task RegisterAsync_WhenUserAlreadyExists_ShouldReturnFalse()
        {
            // Arrange
            var context = CreateDbContext();
            var jwtOptions = CreateJwtOptions();

            context.Users.Add(new User
            {
                UserName = "testuser",
                Email = "test@test.com",
                PasswordHash = "hash"
            });
            await context.SaveChangesAsync();

            var service = new UserService(context, jwtOptions);

            // Act
            var result = await service.RegisterAsync(
                "testuser",
                "test@test.com",
                "123456"
            );

            // Assert
            result.Should().BeFalse();
            context.Users.Count().Should().Be(1);
        }
        [Fact]
        public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnToken()
        {
            var context = CreateDbContext();
            var service = new UserService(context, CreateJwtOptions());

            
            var registered = await service.RegisterAsync("user1", "user1@mail.com", "pass123");
            registered.Should().BeTrue();

            var token = await service.LoginAsync("user1", "pass123");

            token.Should().NotBeNull();
            token.Should().NotBeEmpty();
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordWrongOrUserNotFound_ShouldReturnNull()
        {
            var context = CreateDbContext();
            var service = new UserService(context, CreateJwtOptions());

            await service.RegisterAsync("user1", "user1@mail.com", "pass123");

            var tokenWrongPass = await service.LoginAsync("user1", "WRONG");
            tokenWrongPass.Should().BeNull();

            var tokenNoUser = await service.LoginAsync("no_such_user", "pass123");
            tokenNoUser.Should().BeNull();
        }


    }
}
