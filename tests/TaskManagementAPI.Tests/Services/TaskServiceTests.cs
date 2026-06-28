using Moq;
using FluentAssertions;
using TaskManagementAPI.Application.Services;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Application.Interfaces.Repositories;
using TaskManagementAPI.Application.DTOs.Tasks;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Domain.Enums;

namespace TaskManagementAPI.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTaskDto_WhenTaskExists()
        {
            // Arrange
            var task = new TaskItem
            {
                Id = 1,
                Title = "Test Task",
                Description = "Test Description",
                Status = TaskState.Todo,
                ProjectId = 1
            };
            _taskRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Title.Should().Be("Test Task");
            result.Status.Should().Be("Todo");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskService.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ReturnsTaskDto_WhenTaskIsCreated()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "New Task",
                Description = "New Description",
                ProjectId = 1,
                AssignedToId = null
            };

            _taskRepositoryMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
            _taskRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _taskService.CreateAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("New Task");
            result.Status.Should().Be("Todo");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenTaskExists()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Task to delete" };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
            _taskRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _taskService.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            task.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenTaskDoesNotExist()
        {
            // Arrange
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskService.DeleteAsync(1);

            // Assert
            result.Should().BeFalse();
        }
    }
}