using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;

namespace Boilerplate.Application.Features.Books.UpdateBook;

public class UpdateBookRequest : IRequest<Result>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime PublishedDate { get; set; }

    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public List<string> Genres { get; set; } = new();
}
