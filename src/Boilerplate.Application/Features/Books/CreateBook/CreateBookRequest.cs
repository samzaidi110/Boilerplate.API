using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using System;
using Boilerplate.Domain.Entities;

namespace Boilerplate.Application.Features.Books.CreateBook;

public class CreateBookRequest : IRequest<Result<GetBookResponse>>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime PublishedDate { get; set; }

    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public List<string> Genres { get; set; } = new();
    //public Author Author { get; set; } = null!;

    //public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}
