using Ardalis.Result;
using Boilerplate.Application.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Boilerplate.Application.Features.Books.GetBookById;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdRequest, Result<GetBookByIdResponse>>
{
    private readonly IContext _context;

    public GetBookByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetBookByIdResponse>> Handle(GetBookByIdRequest request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (book == null)
        {
            return Result.NotFound();
        }

        var response = new GetBookByIdResponse
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            AuthorName = book.Author?.Name,
            Genres = book.BookGenres?
                .Select(bg => bg.Genre?.Name ?? string.Empty)
                .ToList() ?? new List<string>()
        };

        return Result.Success(response);
    }
}
