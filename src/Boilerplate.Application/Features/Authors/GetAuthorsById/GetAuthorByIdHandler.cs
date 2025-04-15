using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Application.Features.Authors.GetAuthorsById;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Authors.GetAuthorById;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdRequest, Result<GetAuthorByIdResponse>>
{
    private readonly IContext _context;

    public GetAuthorByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetAuthorByIdResponse>> Handle(GetAuthorByIdRequest request, CancellationToken cancellationToken)
    {
        var author = await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (author == null)
        {
            return Result.NotFound();
        }

        var response = author.Adapt<GetAuthorByIdResponse>();
        return Result.Success(response);
    }
}
