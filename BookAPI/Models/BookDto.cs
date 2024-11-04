namespace BookAPI.Models;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "New Title";
    public string? Description { get; set; }
    public int Year { get; set; }
    public AuthorDto? Author { get; set; }
    public CategoryDto? Category { get; set; }
}