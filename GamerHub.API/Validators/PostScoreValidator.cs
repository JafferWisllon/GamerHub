using FluentValidation;
using GamerHub.API.Models.Dtos;

namespace GamerHub.API.Validators;

public class PostScoreValidator : AbstractValidator<PostScore>
{
    public PostScoreValidator()
    {
        RuleFor(item => item.Score)
            .GreaterThan(0);
    }
}