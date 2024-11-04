namespace BookAPI.Models;

public class BookDtoAdd
{
    public int Id { get; set; }
    public string Title { get; set; } = "ExampleTitle";
    public string? Description { get; set; }
    public int Year { get; set; }
    public int AuthorId { get; set; }
    public int CategoryId { get; set; }
}