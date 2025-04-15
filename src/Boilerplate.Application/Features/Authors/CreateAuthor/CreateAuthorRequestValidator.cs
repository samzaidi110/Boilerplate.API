using FluentValidation;

namespace Boilerplate.Application.Features.Authors.CreateAuthor;

public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Author name is required.")
            .MaximumLength(100)
            .WithMessage("Author name must be 100 characters or fewer.");
    }
}
