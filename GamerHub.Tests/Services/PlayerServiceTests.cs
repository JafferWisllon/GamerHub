using GamerHub.API.Exceptions;
using GamerHub.API.Models.Dtos;
using GamerHub.API.Services;
using GamerHub.API.Services.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

    [Fact]
    public async Task Should_Return_Error_When_Invalid_Score_Is_Provided()
    {
        // Arrange
        var playerId = 123;
        var request = new PostScore() { Score = -10 };
        
        // Act
        var result = await _sut.PostScore(playerId, request);
        
        // Assert
        result.isSuccess.ShouldBe(false);
        result.exceptionViewModel?.Errors.Count.ShouldBe(1);    
    }
    
    [Fact]
    public async Task Should_Return_TooManyRequest_When_has_Locked_Player()
    {
        // Arrange
        var playerId = 123;
        var request = new PostScore() { Score = 10 };
        _cacheService.GetString<bool>(Arg.Any<string>()).Returns(true);

        // Assert
        await Assert.ThrowsAsync<TooManyRequestException>(async () => await _sut.PostScore(playerId, request));
    }
    
    [Fact]
    public async Task Should_Update_Score_Player_WithSuccess()
    {
        // Arrange
        var playerId = 123;
        var request = new PostScore() { Score = 10 };
        _cacheService.GetString<bool>(Arg.Any<string>()).Returns(false);
        
        // Act
        var result = await _sut.PostScore(playerId, request);
        
        // Assert
        result.isSuccess.ShouldBe(true);
        await _cacheService.Received().AddSortedSet(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<double>());
        await _cacheService.Received().SetString(Arg.Any<string>(), Arg.Any<bool>());
    }
}