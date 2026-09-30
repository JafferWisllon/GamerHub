using GamerHub.API.Models.Dtos;
using GamerHub.API.Validators;
using Shouldly;

namespace GamerHub.Tests.Validators;

public class PostPlayerValidatorTests
{
    private readonly PostPlayerValidator _sut;

    public PostPlayerValidatorTests() 
        => _sut = new PostPlayerValidator();

    [Fact]
    public void Should_Return_An_Error_When_Invalid_Id_Is_Provided()
    {
        // Arrange
        var request = new PostPlayer()
        {
            Id = "",
            Name = "John Doe",
            Country = "BR",
            AvatarUrl = "profile.png"
        };
        // Act
        var result = _sut.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.First().PropertyName.ShouldBe("Id");
    }
    
    [Fact]
    public void Should_Return_An_Error_When_Invalid_Name_Is_Provided()
    {
        // Arrange
        var request = new PostPlayer()
        {
            Id = "123",
            Name = string.Empty,
            Country = "BR",
            AvatarUrl = "profile.png"
        };
        // Act
        var result = _sut.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.First().PropertyName.ShouldBe("Name");
    }
    
    [Fact]
    public void Should_Return_An_Error_When_Invalid_Country_Is_Provided()
    {
        // Arrange
        var request = new PostPlayer()
        {
            Id = "123",
            Name = "John",
            Country = " ",
            AvatarUrl = "profile.png"
        };
        // Act
        var result = _sut.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.First().PropertyName.ShouldBe("Country");
    }
    
    [Fact]
    public void Should_Return_An_Error_When_Invalid_Avatar_Is_Provided()
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
        var result = _sut.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
        result.Errors.First().PropertyName.ShouldBe("AvatarUrl");
    }

    [Fact]
    public void Should_Not_Return_Error_When_A_Valid_Request_Is_Provided()
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
        var result = _sut.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(true);
        result.Errors.Count.ShouldBe(0);
    }
}