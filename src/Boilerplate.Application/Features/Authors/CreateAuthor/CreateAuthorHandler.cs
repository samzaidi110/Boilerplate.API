using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace Boilerplate.Application.Features.Authors.CreateAuthor;

public class CreateAuthorHandler : IRequestHandler<CreateAuthorRequest, Result<CreateAuthorResponse>>
{
    private readonly IContext _context;

    public CreateAuthorHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateAuthorResponse>> Handle(CreateAuthorRequest request, CancellationToken cancellationToken)
    {
        var existing = await _context.Authors
            .FirstOrDefaultAsync(a => a.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (existing != null)
        {
            return Result.Conflict("Author already exists.");
        }

        var author = request.Adapt<Author>();
        _context.Authors.Add(author);
        await _context.SaveChangesAsync(cancellationToken);

        var response = author.Adapt<CreateAuthorResponse>();
        return Result.Success(response);
    }
}
