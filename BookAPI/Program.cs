using BookAPI.Data;
using BookAPI.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("BookDbContextConnection") ??
                       throw new InvalidOperationException("No connection string found.");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllRequests", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB Services
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlite(connectionString));

// Interfaces
builder.Services.AddTransient<IBookService, BookService>();

var app = builder.Build();

app.UseCors("AllowAllRequests");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();