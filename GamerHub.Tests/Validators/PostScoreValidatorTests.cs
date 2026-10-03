using GamerHub.API.Models.Dtos;
using GamerHub.API.Validators;
using Shouldly;

namespace GamerHub.Tests.Validators;

public class PostScoreValidatorTests
{
    private readonly PostScoreValidator _sut;

    public PostScoreValidatorTests() 
        => _sut = new PostScoreValidator();

    [Fact]
    public void Should_Return_An_Error_If_Score_Less_Than_Zero()
    {
        // Arrange
        var request = new PostScore() { Score = -10 };
        
        // Act
        var result = _sut.Validate(request);
        
        // Assert
        result.IsValid.ShouldBe(false);
    }
}