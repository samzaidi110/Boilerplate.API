using Ardalis.Result;
using Boilerplate.Application.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Authors.GetAllAuthors;

public class GetAllAuthorsHandler : IRequestHandler<GetAllAuthorsRequest, Result<List<GetAllAuthorsResponse>>>
{
    private readonly IContext _context;

    public GetAllAuthorsHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<List<GetAllAuthorsResponse>>> Handle(GetAllAuthorsRequest request, CancellationToken cancellationToken)
    {
        var authors = await _context.Authors
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

        var response = authors.Adapt<List<GetAllAuthorsResponse>>();
        return Result.Success(response);
    }
}
