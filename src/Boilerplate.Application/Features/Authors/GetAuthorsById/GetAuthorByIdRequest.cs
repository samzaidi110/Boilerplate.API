using Ardalis.Result;
using Boilerplate.Application.Features.Authors.GetAuthorsById;
using MediatR;

namespace Boilerplate.Application.Features.Authors.GetAuthorById;

public class GetAuthorByIdRequest : IRequest<Result<GetAuthorByIdResponse>>
{
    public int Id { get; set; }

    public GetAuthorByIdRequest(int id)
    {
        Id = id;
    }
}
