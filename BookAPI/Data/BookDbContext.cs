using BookAPI.Models;
using BookAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // SEEDING
        
        // Authors
        modelBuilder.Entity<Author>().HasData(new Author { Id = 1, FirstName = "Bill", LastName = "Hopper"});
        modelBuilder.Entity<Author>().HasData(new Author { Id = 2, FirstName = "Trude", LastName = "Moe"});
        modelBuilder.Entity<Author>().HasData(new Author { Id = 3, FirstName = "Ada", LastName = "Love"});
        
        // Categories
        modelBuilder.Entity<Category>().HasData(new Category { Id = 1, CategoryName = "Horror"});
        modelBuilder.Entity<Category>().HasData(new Category { Id = 2, CategoryName = "Fantasy"});
        modelBuilder.Entity<Category>().HasData(new Category { Id = 3, CategoryName = "Crime"});
        modelBuilder.Entity<Category>().HasData(new Category { Id = 4, CategoryName = "Romance"});
        modelBuilder.Entity<Category>().HasData(new Category { Id = 5, CategoryName = "Historical"});
        
        // Books
        modelBuilder.Entity<Book>().HasData(new Book
        {
            Id = 1,
            Title = "The Webpocalypse",
            Description = "Set 50 years after the collapse of the internet",
            Year = 2024,
            AuthorId = 3,
            CategoryId = 1
        });
        
        modelBuilder.Entity<Book>().HasData(new Book
        {
            Id = 2,
            Title = "The Fabled Forest",
            Description = "A mystical forest village rebels against the mountain village",
            Year = 2023,
            AuthorId = 2,
            CategoryId = 2
        });
        
        modelBuilder.Entity<Book>().HasData(new Book
        {
            Id = 3,
            Title = "The forest murders",
            Description = "A fae is found murdered in a forest village. Can detective Fungus solve the crime?",
            Year = 2024,
            AuthorId = 2,
            CategoryId = 2
        });
        
        modelBuilder.Entity<Book>().HasData(new Book
        {
            Id = 4,
            Title = "50 Fabulous Castles",
            Description = "We look into the rich histories of 50 castles in Scotland",
            Year = 2022,
            AuthorId = 1,
            CategoryId = 5
        });
        
        modelBuilder.Entity<Book>().HasData(new Book
        {
            Id = 5,
            Title = "Ben & Jerries",
            Description = "A true love story",
            Year = 2020,
            AuthorId = 1,
            CategoryId = 4
        });
        
        modelBuilder.Entity<Book>().HasData(new Book
        {
            Id = 6,
            Title = "A cook, a fork and a kitchen",
            Description = "Detective Forkster has a hard time solving the case of a missing steak.",
            Year = 2023,
            AuthorId = 3,
            CategoryId = 3
        });

    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
}