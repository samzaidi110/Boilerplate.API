using FluentValidation;
using System;

namespace Boilerplate.Application.Features.Books.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        //RuleFor(x => x.AuthorId).GreaterThan(0);
        RuleFor(x => x.PublishedDate).LessThanOrEqualTo(DateTime.UtcNow);
    }
}