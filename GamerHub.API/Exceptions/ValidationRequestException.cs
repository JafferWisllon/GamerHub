using FluentValidation.Results;

namespace GamerHub.API.Exceptions;

public class ValidationRequestException
{
    public IList<ValidationException> Errors { get; set; } = new List<ValidationException>();

    public static ValidationRequestException CreateViewModelErrors(List<ValidationFailure> validationErrors)
    {
        var errors = new ValidationRequestException();
        foreach (var error in validationErrors)
            errors.Errors.Add(new ValidationException(){Title = error.PropertyName, Message = error.ErrorMessage});
        return errors;
    }
}

public class ValidationException
{
    public string Title { get; set; }
    public string Message { get; set; }
}