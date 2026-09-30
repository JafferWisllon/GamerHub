using GamerHub.API.Models.Dtos;
using GamerHub.API.Services;
using GamerHub.API.Services.Interfaces;
using NSubstitute;
using Shouldly;
using StackExchange.Redis;

namespace GamerHub.Tests.Services;

public class PlayerServiceTests
{
    private readonly PlayerService _sut;
    private readonly ICacheService _cacheService;

    public PlayerServiceTests()
    {
        _cacheService = Substitute.For<ICacheService>();
        _sut = new PlayerService(_cacheService);
    }

    [Fact]
    public async Task Should_Return_Error_Invalid_Request()
    {
        // Arrange
        var request = new PostPlayer()
        {
            Id = "123",
            Name = "John",
            Country = "BR",
            AvatarUrl = ""
        };
        // Act
        var result = await _sut.PostPlayer(request);
        
        // Assert
        result.isSuccess.ShouldBe(false);
        result.exceptionViewModel?.Errors.Count.ShouldBe(1);
    }
    
    [Fact]
    public async Task Should_Return_Success_Valid_Request()
    {
        // Arrange
        var request = new PostPlayer()
        {
            Id = "123",
            Name = "John",
            Country = "BR",
            AvatarUrl = "profile.jpg"
        };
        // Act
        var result = await _sut.PostPlayer(request);
        
        // Assert
        result.isSuccess.ShouldBe(true);
        result.exceptionViewModel.ShouldBeNull();
        await _cacheService.Received().SetHash(Arg.Any<string>(), Arg.Any<HashEntry[]>());
    }
}