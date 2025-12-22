using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjectHub.Infrastructure.Data;
using ProjectHub.Infrastructure.Services;
using ProjectHub.Domin.Entites;
using ProjectHub.Application.Dtos;

namespace ProjectHub.Tests
{
    public class ProjectServiceTests
    {
        private AppDbContext CreateDbContext()

        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
        private async Task<Project> SeedProjectAsync(
            AppDbContext context,
            string name = "Test project")
        {
            var project = new Project
            {
                Name = name

            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();
            return project;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnProjects()
        {
            var context = CreateDbContext();

            context.Projects.AddRange(
                new Project { Name = "Project 1" },
                new Project { Name = "Project 2" });
            await context.SaveChangesAsync();

            var service = new ProjectService(context);

            var result = await service.GetAllAsync(
                page: 1,
                pageSize: 10,
                search: null,
                sortBy: null,
                sortDir: null);
            result.Should().NotBeNull();
            result.Items.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetByIdAsunc_WhenProjectExists_ShouldReturnProject()
        {
            var context = CreateDbContext();
            var project = new Project
            {
                Name = "Project 1"
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var service = new ProjectService(context);

            var result = await service.GetByIdAsync(project.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(project.Id);
            result.Name.Should().Be("Project 1");
        }

        [Fact]
        public async Task GetByIdAsync_WhenProjectDoesNotExist_ShouldReturnNull()
        {
            var context = CreateDbContext();
            var service = new ProjectService(context);

            var result = await service.GetByIdAsync(999);
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_WhenValidDto_ShouldCreateProjectAndReturnId()
        {
            var context = CreateDbContext();
            var service = new ProjectService(context);
            var dto = new ProjectDto
            {
                Name = "Test Project",
                Description = "Test Description",
                CreatedAt = DateTime.UtcNow,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(10)
            };
            var projectId = await service.CreateAsync(dto);
            projectId.Should().BeGreaterThan(0);

            var projectInDb = await context.Projects.FindAsync(projectId);
            projectInDb.Should().NotBeNull();
            projectInDb!.Name.Should().Be("Test Project");
        }
        [Fact]
        public async Task UpdateAsync_WhenProjectExists_ShouldUpdateAndReturnTrue()
        {
            // Arrange
            var context = CreateDbContext();

            var project = new Project
            {
                Name = "Old Name",
                Description = "Old Desc",
                StartDate = DateTime.UtcNow.Date,
                EndDate = DateTime.UtcNow.Date.AddDays(1)
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var service = new ProjectService(context);

            var dto = new ProjectDto
            {
                Name = "New name",
                Description = "New Desc",
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };

            // Act
            var result = await service.UpdateAsync(project.Id, dto);

            // Assert
            result.Should().BeTrue();

            var updated = await context.Projects.FindAsync(project.Id);
            updated.Should().NotBeNull();
            updated!.Name.Should().Be("New name");
            updated.Description.Should().Be("New Desc");
        }

        [Fact]
        public async Task DeleteAsync_WhenExists_ShouldDeeleteAndReturnTrue()
        {
            var context = CreateDbContext();
            var project = await SeedProjectAsync(context);

            var service = new ProjectService(context);
            var result = await service.DeleteAsync(project.Id);

            result.Should().BeTrue();
            context.Projects.Count().Should().Be(0);
        }

    }
}
