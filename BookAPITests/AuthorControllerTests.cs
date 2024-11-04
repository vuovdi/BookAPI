using BookAPI.Controllers;
using BookAPI.Models;
using BookAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookAPITests;

public class AuthorControllerTests
{
    private readonly Mock<IBookService> _mock;
    private readonly AuthorController _controller;

    public AuthorControllerTests()
    {
        _mock = new Mock<IBookService>();
        _controller = new AuthorController(_mock.Object);
        
    }
    
    [Fact]
    public async Task GetAuthors_ReturnsOkWithAuthorsList()
    {
        // Arrange
        var authors = new List<AuthorDto>
        {
            new AuthorDto { Id = 1, FirstName = "Bill", LastName = "Hopper" },
            new AuthorDto { Id = 2, FirstName = "Trude", LastName = "Moe" }
        };
        _mock.Setup(service => service.GetAllAuthors()).ReturnsAsync(authors);

        // Act
        var result = await _controller.GetAuthors();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(authors, okResult.Value);
    }
    
    [Fact]
    public void Get_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Id", "Id is required");

        // Act
        var result = _controller.Get(1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Get_ReturnsNotFound_WhenAuthorDoesNotExist()
    {
        // Arrange
        _mock.Setup(service => service.GetAuthor(1)).Returns((AuthorDto)null);

        // Act
        var result = _controller.Get(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No author with Id 1 was found.", notFoundResult.Value);
    }

    [Fact]
    public void Get_ReturnsOkWithAuthor_WhenAuthorExists()
    {
        // Arrange
        var author = new AuthorDto { Id = 1, FirstName = "Bill", LastName = "Hopper" };
        _mock.Setup(service => service.GetAuthor(1)).Returns(author);

        // Act
        var result = _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(author, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("FirstName", "FirstName is required");
        var author = new AuthorDto { Id = 1, FirstName = "", LastName = "Doe" };

        // Act
        var result = await _controller.Create(author);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenAuthorAlreadyExists()
    {
        // Arrange
        var author = new AuthorDto { Id = 1, FirstName = "Bill", LastName = "Hopper" };
        _mock.Setup(service => service.GetAuthor(1)).Returns(new AuthorDto { Id = 1 });

        // Act
        var result = await _controller.Create(author);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal("Author with Id 1 already exists.", conflictResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenAuthorIsCreatedSuccessfully()
    {
        // Arrange
        var author = new AuthorDto { Id = 1, FirstName = "Bill", LastName = "Hopper" };
        _mock.Setup(service => service.GetAuthor(1)).Returns((AuthorDto)null);
        _mock.Setup(service => service.SaveAuthor(It.IsAny<Author>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(author);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("Get", createdAtActionResult.ActionName);
        Assert.NotNull(createdAtActionResult.RouteValues);
    }
}