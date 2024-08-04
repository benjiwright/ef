using Dometrain.EFCore.SimpleAPI.Controllers;
using Dometrain.EFCore.SimpleAPI.Data;
using Dometrain.EFCore.SimpleAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace Dometrain.EFCore.Tests.FakeDbSet;

public class FakeDbSetTest
{
    [Fact]
    public async Task IfGenreExists_ReturnsGenre()
    {
        // Arrange
        var mockContext = CreateFakeDbContext();
        var controller = new GenresController(mockContext);
        
        // Act
        var response = await controller.Get(2);
        var okResult = response as OkObjectResult;
        
        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Action", (okResult.Value as Genre)?.Name);
        await mockContext.DidNotReceive().SaveChangesAsync();
    }

    private static MoviesContext CreateFakeDbContext()
    {
        List<Genre> genres =
        [
            new Genre { Id = 1, Name = "Drama" },
            new Genre { Id = 2, Name = "Action" },
            new Genre { Id = 3, Name = "Comedy" }
        ];

        // create context mock
        var mockContext = Substitute.For<MoviesContext>();

        // build DbSet
        var mockGenresSet = genres.AsQueryable().BuildMockDbSet();

        // if our controller called FirstOrDefaultAsync, we would not need to mock this because
        // that free out of the box with MockQueryable.NSubstitute
        // but our controller uses FindAsync instead, so we need to explicitly mock it
        mockGenresSet.FindAsync(2)!
            .Returns(new ValueTask<Genre>(genres.ElementAt(1)));

        // return our fake set
        mockContext.Genres.Returns(mockGenresSet);

        return mockContext;
    }
}