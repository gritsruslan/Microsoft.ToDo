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

public class CategoryServiceShould
{
    private readonly CategoryService _categoryService;

    private readonly Mock<ICategoryRepository> _repositoryMock;

    public CategoryServiceShould()
    {
        _repositoryMock = new Mock<ICategoryRepository>();

        var validatorMock = new Mock<IValidator<CreateCategoryRequest>>();

        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCategoryRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _categoryService = new CategoryService(
            _repositoryMock.Object,
            validatorMock.Object);
    }

    [Fact]
    public async Task ThrowUnauthorizedExceptionWhileCreateCategory_IfUserIsNotAuthenticated()
    {
        await _categoryService.Invoking(s =>
                s.CreateCategory(new CreateCategoryRequest("Work"), null, CancellationToken.None))
            .Should()
            .ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task SuccessfullyCreateCategory()
    {
        var userId = "SuccessfullyCreateCategory";
        var categoryName = "Work";

        _repositoryMock.Setup(r => r.Create(categoryName, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category
            {
                Id = 1,
                Name = categoryName,
                UserId = userId,
                User = null!
            });

        var category = await _categoryService.CreateCategory(
            new CreateCategoryRequest(categoryName), userId, CancellationToken.None);

        _repositoryMock.Verify(r => r.Create(
            categoryName, userId, It.IsAny<CancellationToken>()), Times.Once);

        category.Should().BeEquivalentTo(
            new CategoryResponse(1, categoryName));
    }

    [Fact]
    public async Task ThrowUnauthorizedExceptionWhileGetAllCategories_IfUserIsNotAuthenticated()
    {
        await _categoryService.Invoking(s => s.GetAllCategories(null, CancellationToken.None))
            .Should()
            .ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task SuccessfullyGetAllCategories()
    {
        var userId = "SuccessfullyGetAllCategories";

        var categories = new List<Category>
        {
            new()
            {
                Id = 1,
                Name = "Work",
                UserId = userId,
                User = null!
            },
            new()
            {
                Id = 2,
                Name = "Home",
                UserId = userId,
                User = null!
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllByUser(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        var result = await _categoryService.GetAllCategories(userId, CancellationToken.None);
        
        result.Should().BeEquivalentTo(
        [
            new CategoryResponse(1, "Work"),
            new CategoryResponse(2, "Home")
        ]);
    }

    [Fact]
    public async Task ReturnEmptyCollectionWhileGetAllCategories_WhenUserHasNoCategories()
    {
        var userId = "ReturnEmptyCollectionWhileGetAllCategories_WhenUserHasNoCategories";

        _repositoryMock
            .Setup(r => r.GetAllByUser(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _categoryService.GetAllCategories(userId, CancellationToken.None);

        result.Should().BeEmpty();
    }
}