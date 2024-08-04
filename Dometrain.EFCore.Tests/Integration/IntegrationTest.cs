using Dometrain.EFCore.SimpleAPI.Data;
using Dometrain.EFCore.SimpleAPI.Models;
using Dometrain.EfCore.SimpleAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.Tests.Integration;

public class IntegrationTest
{
    private readonly MoviesContext _testContext;
    private readonly MoviesContext _verificationContext;

    public IntegrationTest()
    {
        // connecting to a REAL test database
        var options = new DbContextOptionsBuilder<MoviesContext>()
            .UseSqlServer("""
                          Data Source=localhost,1420;
                          Initial Catalog=TestDB;
                          User Id=sa;
                          Password=MySaPassword123;
                          TrustServerCertificate=True;
                          """)
            .Options;

        // pointing at the same database
        _testContext = new MoviesContext(options);
        _verificationContext = new MoviesContext(options);

        // Usually not needed, as a deploy of the test database
        // before the test run will migrate the schema.
        _testContext.Database.EnsureDeleted();
        _testContext.Database.EnsureCreated();
    }
    
    [Fact]
    public async Task WhenGenreCreated_GenreIsInDatabase()
    {
        // Approach:
        // 1) use one context to create a genre
        // 2) use other context to verify that the genre is in the database
        
        // Arrange
        var repository = new GenreRepository(_testContext);
        
        // Act
        var genreToCreate = new Genre { Name = "NewGenre" };
        var createdGenre = await repository.Create(genreToCreate);
        
        // Assert
        Assert.NotNull(createdGenre);
        Assert.True(createdGenre.Id > 0);
        var verificationGenre = await _verificationContext.Genres.FindAsync(createdGenre.Id);
        Assert.NotNull(verificationGenre);
        Assert.Equal(genreToCreate.Name, verificationGenre.Name);
    }
}