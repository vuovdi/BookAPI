using BookAPI.Controllers;
using BookAPI.Models;
using BookAPI.Models.Entities;
using BookAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookAPITests;

public class BookControllerTests
{
    private readonly Mock<IBookService> _mock;
    private readonly BookController _controller;

    public BookControllerTests()
    {
        _mock = new Mock<IBookService>();
        _controller = new BookController(_mock.Object);
    }

    private static List<BookDto> GetTestBooks()
    {
        return new List<BookDto>
        {
            new BookDto
            { Id = 1, Title = "Book 1", Description = "About", Year = 2022 },
            new BookDto
            { Id = 2, Title = "Book 2", Description = "About", Year = 2023 },
            new BookDto { Id = 3, Title = "Book 3", Description = "About", Year = 2022}
        };
    }
    
    // GET
    [Fact]
    public async Task GetAll_ReturnsCorrectType()
    {
        // Arrange
        _mock.Setup(service => service.GetAllBooks()).ReturnsAsync(GetTestBooks);
        
        // Act
        var result = await _controller.GetBooks();
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public void GetBook_returnsOkObjectResult_whenBookExists()
    {
        // Arrange
        var book = new BookDto { Id = 1, Title = "Book 1", Description = "About", Year = 2022 };
        _mock.Setup(service => service.GetBook(1)).Returns(book);
        
        // Act
        var result = _controller.Get(1) as OkObjectResult;
        
        // Assert
        Assert.NotNull(result);
        if (result != null) Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public void GetBook_returnsNotFoundObjectResult_whenBookDoesntExists()
    {
        // Arrange
        var book = new BookDto { Id = 1, Title = "Book 1", Description = "About", Year = 2022 };
        _mock.Setup(service => service.GetBook(1)).Returns(book);
        
        // Act
        var result = _controller.Get(2) as NotFoundObjectResult;
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public void GetBook_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Id", "Invalid Id");

        // Act
        var result = _controller.Get(1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    // Post
    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = await _controller.Create(new BookDtoAdd());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Create_ReturnsConflict_WhenBookWithSameTitleExists()
    {
        // Arrange
        var existingBook = new BookDto { Title = "Existing Book" };
        var newBook = new BookDtoAdd { Title = "Existing Book", Description = "About", Year = 2022 };

        _mock.Setup(service => service.GetBookByTitle("Existing Book")).Returns(existingBook);

        // Act
        var result = await _controller.Create(newBook);

        // Assert
        Assert.IsType<ConflictObjectResult>(result);
        if (result is ConflictObjectResult conflictResult)
        {
            Assert.Equal("A book with the same title already exists!", conflictResult.Value);
        }
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WhenBookIsCreatedSuccessfully()
    {
        // Arrange
        var newBook = new BookDtoAdd { Title = "New Book", Description = "About", Year = 2022, AuthorId = 1, CategoryId = 2 };

        _mock.Setup(service => service.GetBookByTitle("New Book")).Returns((BookDto)null);
        _mock.Setup(service => service.Save(It.IsAny<Book>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(newBook);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("Get", createdAtActionResult.ActionName);
        Assert.Equal(newBook.Title, ((Book)createdAtActionResult.Value).Title);
    }
    
    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var bookDto = new BookDtoAdd { Id = 1, Title = "Book 1" };

        // Act
        var result = await _controller.Update(2, bookDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        if (result is BadRequestObjectResult badRequestResult)
        {
            Assert.Equal("Id from route does not match id from body.", badRequestResult.Value);
        }
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Title is required");
        var bookDto = new BookDtoAdd { Id = 1, Title = "" };

        // Act
        var result = await _controller.Update(1, bookDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        var bookDto = new BookDtoAdd { Id = 1, Title = "Non-Existent Book" };
        _mock.Setup(service => service.GetBook(1)).Returns((BookDto)null);

        // Act
        var result = await _controller.Update(1, bookDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        if (result is NotFoundObjectResult notFoundResult)
        {
            Assert.Equal("No book found with id 1!", notFoundResult.Value);
        }
    }

    [Fact]
    public async Task Update_ReturnsOk_WhenBookIsUpdatedSuccessfully()
    {
        // Arrange
        var bookDto = new BookDtoAdd { Id = 1, Title = "Updated Book", Description = "Updated", Year = 2023 };
        var existingBook = new BookDto { Id = 1, Title = "Old Title" };

        _mock.Setup(service => service.GetBook(1)).Returns(existingBook);
        _mock.Setup(service => service.Save(It.IsAny<Book>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(1, bookDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        
    }
    
    [Fact]
    public void Delete_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        _mock.Setup(service => service.GetBook(1)).Returns((BookDto)null);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        if (result is NotFoundObjectResult notFoundResult)
        {
            Assert.Equal("No book found with id 1!", notFoundResult.Value);
        }
    }

    [Fact]
    public void Delete_ReturnsOk_WhenBookIsDeletedSuccessfully()
    {
        // Arrange
        var book = new BookDto { Id = 1, Title = "Book to Delete" };
        _mock.Setup(service => service.GetBook(1)).Returns(book);

        // Act
        var result = _controller.Delete(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Book deleted with id 1!", okResult.Value);
    }

}