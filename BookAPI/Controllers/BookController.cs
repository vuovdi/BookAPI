using BookAPI.Models;
using BookAPI.Models.Entities;
using BookAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var result = await _service.GetAllBooks();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var b = _service.GetBook(id);
        if (b == null)
        {
            return NotFound($"No book found with id {id}!");
        }
        
        return Ok(b);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookDtoAdd book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingBook = _service.GetBookByTitle(book.Title);
        if (existingBook != null)
        {
            return Conflict("A book with the same title already exists!");
        }

        var newBook = new Book
        {
            Title = book.Title,
            Description = book.Description,
            Year = book.Year,
            AuthorId = book.AuthorId,
            CategoryId = book.CategoryId
        };

        await _service.Save(newBook);
        return CreatedAtAction("Get", new { id = book.Id }, newBook);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BookDtoAdd book)
    {
        if (id != book.Id)
            return BadRequest("Id from route does not match id from body.");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingBook = _service.GetBook(id);
        if (existingBook == null)
        {
            return NotFound($"No book found with id {id}!");
        }

        var updatedBook = new Book
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Year = book.Year,
            AuthorId = book.AuthorId,
            CategoryId = book.CategoryId
        };
        await _service.Save(updatedBook);
        return Ok(new { Message = $"Book updated with id {id}!", Book = updatedBook });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var book = _service.GetBook(id);
        if (book == null)
            return NotFound($"No book found with id {id}!");

        _service.Delete(id);
        return Ok($"Book deleted with id {id}!");
    }
    
}