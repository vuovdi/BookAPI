using BookAPI.Models;
using BookAPI.Models.Entities;

namespace BookAPI.Service;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllBooks();
    Task<IEnumerable<AuthorDto>> GetAllAuthors();
    BookDto? GetBook(int id);
    BookDto? GetBookByTitle(string title);
    AuthorDto? GetAuthor(int id);
    Task Save(Book book);
    Task SaveAuthor(Author author);
    Task Delete(int id);
    Task DeleteAuthor(int id);
}