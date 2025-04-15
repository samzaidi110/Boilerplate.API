using Ardalis.Result;
using MediatR;
using System;

namespace Boilerplate.Application.Features.Books.GetBookById;

public class GetBookByIdRequest : IRequest<Result<GetBookByIdResponse>>
{
    public int Id { get; set; }

    public GetBookByIdRequest(int id)
    {
        Id = id;
    }
}
