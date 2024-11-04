namespace BookAPI.Models.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "New Title";
    public string? Description { get; set; }
    public int Year { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}