using Dometrain.EFCore.SimpleAPI.Controllers;
using Dometrain.EFCore.SimpleAPI.Models;
using Dometrain.EfCore.SimpleAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Dometrain.EFCore.Tests.Repository;

public class RepositoryTest
{
    [Fact]
    public async Task IfGenreExists_ReturnsGenre()
    {
        // Arrange
        var mockRepo = Substitute.For<IGenreRepository>();
        mockRepo.Get(2)!.Returns(Task.FromResult(new Genre { Id = 2, Name = "Action" }));
        var sut = new GenresWithRepositoryController(mockRepo);
        
        // Act
        var response = await sut.Get(2);
        var okResult = response as OkObjectResult;
        
        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("Action", (okResult.Value as Genre)?.Name);
        Assert.Equal(2, (okResult.Value as Genre)?.Id);
        await mockRepo.Received().Get(2);
    }
}