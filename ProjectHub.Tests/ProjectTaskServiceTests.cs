using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectHub.Application.Dtos;
using ProjectHub.Domin.Entites;
using ProjectHub.Infrastructure.Data;
using ProjectHub.Infrastructure.Services;
using System.ComponentModel;
using Xunit;

namespace ProjectHub.Tests
{
    public class ProjectTaskServiceTests
    {
        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnTasks()
        {
            var context = CreateDbContext();
            context.ProjectTasks.Add(new ProjectTask
            {
                Title = "Task 1",
                ProjectId = 1
            });
            context.ProjectTasks.Add(new ProjectTask
            {
                Title = "Task 2",
                ProjectId = 1
            });
            await context.SaveChangesAsync();
            var service = new ProjectTaskService(context);

            var result = await service.GetAllAsync(
                page: 1,
                pageSize: 10,
                search: null,
                projectId: null,
                sortBy: null,
                sortDir: null);
            result.Items.Count.Should().Be(2);
        }
        [Fact]
        public async Task GetAllAsync_WhenSearchProvided_ShouldReturnMatchingTasks()
        {
            var context = CreateDbContext();

            context.ProjectTasks.Add(new ProjectTask
            {
                Title = "Fix login bug",
                ProjectId = 1
            });
            await context.SaveChangesAsync();
            var service = new ProjectTaskService(context);

            var result = await service.GetAllAsync(
                page: 1,
                pageSize: 10,
                search: "login",
                projectId: null,
                sortBy: null,
                sortDir: null
                );
            result.Items.Count.Should().Be(1);
            result.Items[0].Title.Should().Contain("login");
        }

        [Fact]
        public async Task GetAllAsync_WhenProjectIdProvided_ShouldReturnOnlyThatProjectTasks()
        {
            var context = CreateDbContext();
            context.ProjectTasks.AddRange(
                new ProjectTask { Title = "Task A", ProjectId = 1 },
                new ProjectTask { Title = "Task B", ProjectId = 1 },
                new ProjectTask { Title = "Task C", ProjectId = 2 });
            await context.SaveChangesAsync();

            var service = new ProjectTaskService(context);

            var result = await service.GetAllAsync(1, 10, null, 1, null, null);

            result.Items.Should().HaveCount(2);
            result.Items.All(t => t.ProjectId == 1).Should().BeTrue();

        }

        [Fact]
        public async Task GetAllAsync_ShouldApplyPaging()
        {
            var context = CreateDbContext();

            for(int i = 1; i <=12; i++)
            {
                context.ProjectTasks.Add(new ProjectTask
                {
                    Title = $"Task {i}",
                    ProjectId = 1
                });
            }

            await context.SaveChangesAsync();
            var service = new ProjectTaskService(context);

            var page1 = await service.GetAllAsync(1, 10, null, null, null, null);
            var page2 = await service.GetAllAsync(2, 10, null, null, null, null);

            page1.Items.Should().HaveCount(10);
            page2.Items.Should().HaveCount(2);

            page1.TotalCount.Should().Be(12);
            page2.TotalCount.Should().Be(12);
        }
        [Fact]
        public async Task GetAllAsync_WhenSearchProvided_ShouldReturnOnlyMatchingTasks()
        {
            var context = CreateDbContext();
            context.ProjectTasks.AddRange(
                new ProjectTask { Title = "Fix login bug", ProjectId = 1 },
                new ProjectTask { Title = "Write documentation", ProjectId = 1 },
                new ProjectTask { Title = "Login UI polish", ProjectId = 2 });

            await context.SaveChangesAsync();

            var service = new ProjectTaskService(context);

            var result = await service.GetAllAsync(1, 10, "login", null, null, null);

            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Title.ToLower().Contains("login")).Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAsync_DefaultSort_ShouldReturnNewestFirst()
        {
            var context = CreateDbContext();
            context.ProjectTasks.AddRange(
                new ProjectTask { Title = "Old", ProjectId = 1 },
                new ProjectTask { Title = "New", ProjectId = 1 });

            await context.SaveChangesAsync();

            var service = new ProjectTaskService(context);

            var result = await service.GetAllAsync(1, 10, null, null, null, null);

            result.Items.Should().NotBeEmpty();
            result.Items[0].Id.Should().BeGreaterThan(result.Items[1].Id);
        }

       
}
    
}
