using FluentValidation;
using GamerHub.API.Models.Dtos;

namespace GamerHub.API.Validators;

public class PostPlayerValidator : AbstractValidator<PostPlayer>
{
    public PostPlayerValidator()
    {
        RuleFor(player => player.Id)
            .NotEmpty();

        RuleFor(player => player.Name)
            .NotEmpty();
        
        RuleFor(player => player.Country)
            .NotEmpty();
        
        RuleFor(player => player.AvatarUrl)
            .NotEmpty();
    }
}