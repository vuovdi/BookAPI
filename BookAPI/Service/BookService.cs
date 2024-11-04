using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Service;

public class BookService : IBookService
{
    private readonly BookDbContext _db;

    public BookService(BookDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<BookDto>> GetAllBooks()
    {
        try
        {
            var allBooks = _db.Books
                .Include(c => c.Category)
                .Include(c => c.Author)
                .ToList();
            
            var returnBooks = allBooks.Select( c => new BookDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Year = c.Year,
                Author = new AuthorDto
                {
                    Id = c.Author.Id,
                    FirstName = c.Author.FirstName,
                    LastName = c.Author.LastName
                },
                Category = new CategoryDto
                {
                    Id = c.Category.Id,
                    CategoryName = c.Category.CategoryName
                }
            }).ToList();
            
            return returnBooks;
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            
            return new List<BookDto>();
        }  
    }

    public BookDto? GetBook(int id)
    {
        try
        {
            var book = _db.Books
                .Where(c => c.Id == id)
                .Include(c => c.Category)
                .Include(c => c.Author)
                .FirstOrDefault();
            
            if (book == null) return null;
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Year = book.Year,
                Author = new AuthorDto
                {
                    Id = book.Author.Id,
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName
                },
                Category = new CategoryDto
                {
                    Id = book.Category.Id,
                    CategoryName = book.Category.CategoryName
                }
            };
            return bookDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public BookDto? GetBookByTitle(string title)
    {
        try
        {
            var book = _db.Books
                .Where(c => c.Title.ToLower() == title.ToLower())
                .Include(c => c.Category)
                .Include(c => c.Author)
                .FirstOrDefault();
            
            if (book == null) return null;
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Year = book.Year,
                Author = new AuthorDto
                {
                    Id = book.Author.Id,
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName
                },
                Category = new CategoryDto
                {
                    Id = book.Category.Id,
                    CategoryName = book.Category.CategoryName
                }
            };
            return bookDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Authors
    public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
    {
        try
        {
            var authors = _db.Authors
                .ToList();
            
            var returnedAuthors = authors.Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName
            }).ToList();
            return returnedAuthors;
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return new List<AuthorDto>();
        }
    }

    public AuthorDto? GetAuthor(int id)
    {
        try
        {
            var author = _db.Authors.FindAsync(id).Result;
            if (author == null) return null;

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
            return authorDto;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Save(Book book)
    {
        var existingBook = await _db.Books.FindAsync(book.Id);
        if (existingBook != null)
        {
            _db.Entry(existingBook).State = EntityState.Detached;
        }
        
        _db.Books.Update(book);
        await _db.SaveChangesAsync();
    }

    public async Task SaveAuthor(Author author)
    {
        var existingAuthor = await _db.Authors.FindAsync(author.Id);
        if (existingAuthor != null)
        {
            _db.Entry(existingAuthor).State = EntityState.Detached;
        }
        
        _db.Authors.Update(author);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var book = _db.Books.FindAsync(id);
        
        _db.Books.Remove(await book ?? throw new InvalidOperationException($"No book found with id {id}!"));
        await _db.SaveChangesAsync();
    }
    
    public async Task DeleteAuthor(int id)
    {
        var author = _db.Authors.FindAsync(id);
        
        _db.Authors.Remove(await author ?? throw new InvalidOperationException($"No author found with id {id}!"));
        await _db.SaveChangesAsync();
    }
    
    
}