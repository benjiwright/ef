using Dometrain.EFCore.SimpleAPI.Controllers;
using Dometrain.EFCore.SimpleAPI.Data;
using Dometrain.EFCore.SimpleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.Tests.MemoryDatabase;

public class SqlLiteTest : IDisposable
{
    private readonly SqliteConnection _connection;

    public SqlLiteTest()
    {
        // Test setup
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        
        var context = CreateInMemoryContext();
        // create tables
        context.Database.EnsureCreated();
        // seed data
        context.Genres.AddRange(new List<Genre>
        {
            new Genre {Id = 1, Name = "Comedy"},
            new Genre {Id = 2, Name = "Action"},
            new Genre {Id = 3, Name = "Drama"}
        });
        
        context.SaveChanges();
    }
    
    public void Dispose()
    {
        // Test teardown
        _connection.Dispose();
    }
    
    [Fact]
    public async Task IfGenreExists_ReturnsGenre()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var controller = new GenresController(context);
        
        // Act
        var response = await controller.Get(2);
        var okResult = response as OkObjectResult;
        
        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Action", (okResult.Value as Genre)?.Name);
    }

    private MoviesContext CreateInMemoryContext()
    {
        var contextOptions = new DbContextOptionsBuilder<MoviesContext>()
            .UseSqlite(_connection)
            .Options;
        
        var context = new MoviesContext(contextOptions);

        return context;
    }
}