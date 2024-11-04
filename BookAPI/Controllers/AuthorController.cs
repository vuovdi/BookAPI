using BookAPI.Models;
using BookAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class AuthorController : ControllerBase
{
    private readonly IBookService _service;

    public AuthorController(IBookService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        var result = await _service.GetAllAuthors();
        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult Get([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        var a = _service.GetAuthor(id);
        if (a == null)
        {
            return NotFound($"No author with Id {id} was found.");
        }

        return Ok(a);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AuthorDto author)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var existingAuthor = _service.GetAuthor(author.Id);
        if (existingAuthor != null)
        {
            return Conflict($"Author with Id {author.Id} already exists.");
        }

        var newAuthor = new Author
        {
            FirstName = author.FirstName,
            LastName = author.LastName,
        };
        
        await _service.SaveAuthor(newAuthor);
        return CreatedAtAction("Get", new { id = newAuthor.Id }, newAuthor);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var author = _service.GetAuthor(id);
        if (author == null)
            return NotFound($"No author found with id {id}!");

        _service.DeleteAuthor(id);
        return Ok($"Author deleted with id {id}!");
    }
}