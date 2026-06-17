using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.ToDo.Application.Abstraction;
using Microsoft.ToDo.Application.DTOs;
using Microsoft.ToDo.Application.Exceptions;
using Microsoft.ToDo.Application.Services;
using Microsoft.ToDo.Domain.Models;
using Moq;

namespace Microsoft.ToDo.UnitTests;

public class TaskServiceShould
{
    private readonly TaskService _taskService;

    private readonly Mock<ITaskRepository> _taskRepositoryMock;

    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    public TaskServiceShould()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var createValidatorMock = new Mock<IValidator<CreateTaskRequest>>();
        var searchValidatorMock = new Mock<IValidator<SearchTasksRequest>>();
        var updateValidatorMock = new Mock<IValidator<UpdateTaskRequest>>();

        createValidatorMock.Setup(v => v.ValidateAsync(
            It.IsAny<CreateTaskRequest>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        searchValidatorMock.Setup(v => v.ValidateAsync(
            It.IsAny<SearchTasksRequest>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        updateValidatorMock.Setup(v => v.ValidateAsync(
            It.IsAny<UpdateTaskRequest>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _taskService = new TaskService(
            _taskRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            createValidatorMock.Object,
            searchValidatorMock.Object,
            updateValidatorMock.Object);
    }

    [Fact]
    public async Task ThrowCategoryNotFoundExceptionWhileCreateTask_IfCategoryDoesNotExist()
    {
        var userId = "user";
        var categoryId = 1;

        _categoryRepositoryMock.Setup(r =>
            r.GetById(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        await _taskService.Invoking(s => s.CreateTask(
                new CreateTaskRequest("Task", categoryId, null), userId, CancellationToken.None))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
    }

    [Fact]
    public async Task ThrowForbiddenExceptionWhileCreateTask_IfCategoryBelongsToAnotherUser()
    {
        var userId = "user";

        _categoryRepositoryMock.Setup(r =>
            r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category
            {
                Id = 1,
                Name = "Work",
                UserId = "another-user",
                User = null!
            });

        await _taskService.Invoking(s => s.CreateTask(
                new CreateTaskRequest("Task", 1, null), userId, CancellationToken.None))
            .Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task SuccessfullyCreateTask()
    {
        var userId = "user";
        var categoryId = 1;
        DateTimeOffset? dueDate = null;

        _categoryRepositoryMock.Setup(r =>
            r.GetById(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category
            {
                Id = categoryId,
                UserId = userId,
                Name = string.Empty,
                User = null!
            });

        _taskRepositoryMock.Setup(r =>
            r.Create("Task", dueDate, categoryId, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TaskItem
            {
                Id = 5,
                Title = "Task",
                DueDate = dueDate,
                IsCompleted = false,
                CategoryId = categoryId,
                UserId = string.Empty,
                User = null!,
                Category = null!
            });

        var task = await _taskService.CreateTask(
            new CreateTaskRequest("Task", categoryId, dueDate), userId, CancellationToken.None);

        _taskRepositoryMock.Verify(r => r.Create("Task", dueDate, categoryId, userId,
                It.IsAny<CancellationToken>()), Times.Once);

        task.Should().BeEquivalentTo(new TaskResponse(5, "Task", dueDate, false, categoryId));
    }

    [Fact]
    public async Task ThrowUnauthorizedExceptionWhileSearchTasks_IfUserIsNotAuthenticated()
    {
        await _taskService.Invoking(s =>
                s.SearchTasks(
                    new SearchTasksRequest(null, null, 1, 10),
                    null,
                    CancellationToken.None))
            .Should()
            .ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task SuccessfullySearchTasks()
    {
        var userId = "user";

        var category = new Category
        {
            Id = 1,
            Name = "Work",
            UserId = string.Empty,
            User = null!
        };

        var tasks = new List<TaskItem>
        {
            new()
            {
                Id = 1,
                Title = "Task1",
                IsCompleted = false,
                CategoryId = 1,
                Category = category,
                UserId = string.Empty,
                User = null!
            }
        };

        _taskRepositoryMock.Setup(r =>
            r.Search(null, null, userId, 0, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((tasks, 1));

        var result = await _taskService.SearchTasks(
            new SearchTasksRequest(null, null, 1, 10),
            userId,
            CancellationToken.None);

        result.TotalCount.Should().Be(1);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        result.Items.Should().BeEquivalentTo(
        [
            new TaskReadModel(
                1,
                "Task1",
                null,
                false,
                1,
                "Work")
        ]);
    }

    [Fact]
    public async Task ThrowTaskNotFoundExceptionWhileUpdateTask_IfTaskDoesNotExist()
    {
        _taskRepositoryMock.Setup(r =>
            r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        _categoryRepositoryMock.Setup(r => r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category
            {
                Id = 1,
                Name = "Work",
                UserId = null!,
                User = null!
            });
        
        await _taskService.Invoking(s => s.UpdateTask(
                1,
                new UpdateTaskRequest("Task", null, true, 1),
                "user",
                CancellationToken.None))
            .Should()
            .ThrowAsync<TaskNotFoundException>();
    }

    [Fact]
    public async Task SuccessfullyUpdateTask()
    {
        _taskRepositoryMock.Setup(r =>
            r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TaskItem
            {
                Id = 1,
                UserId = "user",
                Title = string.Empty,
                User = null!,
                Category = null!,
            });

        _categoryRepositoryMock.Setup(r => r.GetById(1, It.IsAny<CancellationToken>())).ReturnsAsync(
            new Category
            {
                Name = "category",
                UserId = "user",
                User = null!
            });

        await _taskService.UpdateTask(
            1,
            new UpdateTaskRequest("Updated", null, true, 1),
            "user",
            CancellationToken.None);

        _taskRepositoryMock.Verify(r => r.Update(1, "Updated", null, 
            true, 1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ThrowTaskNotFoundExceptionWhileDeleteTask_IfTaskDoesNotExist()
    {
        _taskRepositoryMock.Setup(r => r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        await _taskService.Invoking(s => s.DeleteTask(1, "user", CancellationToken.None))
            .Should()
            .ThrowAsync<TaskNotFoundException>();
    }

    [Fact]
    public async Task ThrowForbiddenExceptionWhileDeleteTask_IfTaskBelongsToAnotherUser()
    {
        _taskRepositoryMock.Setup(r =>
            r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TaskItem
            {
                Id = 1,
                UserId = "another-user",
                Title = string.Empty,
                User = null!,
                Category = null!
            });

        await _taskService.Invoking(s =>
                s.DeleteTask(1, "user", CancellationToken.None))
            .Should()
            .ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task SuccessfullyDeleteTask()
    {
        _taskRepositoryMock.Setup(r =>
            r.GetById(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TaskItem
            {
                Id = 1,
                UserId = "user",
                Title = string.Empty,
                User = null!,
                Category = null!
            });

        await _taskService.DeleteTask(1, "user", CancellationToken.None);

        _taskRepositoryMock.Verify(r =>
            r.Delete(1, It.IsAny<CancellationToken>()), Times.Once);
    }
}