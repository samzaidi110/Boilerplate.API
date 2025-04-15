using Ardalis.Result;
using MediatR;
using System;

namespace Boilerplate.Application.Features.Books.DeleteBook;

public class DeleteBookRequest : IRequest<Result>
{
    public int Id { get; set; }

    public DeleteBookRequest(int id)
    {
        Id = id;
    }
}
