using Ardalis.Result;
using Boilerplate.Application.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Books.GetAllBooks;
public class GetAllBooksHandler : IRequestHandler<GetAllBooksRequest, Result<List<GetBookResponse>>>
{
    private readonly IContext _context;

    public GetAllBooksHandler(IContext context)
    {
        _context = context;
    }



    public async Task<Result<List<GetBookResponse>>> Handle(GetAllBooksRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .AsQueryable();

        // 🔍 Filter by genre name if provided
        if (!string.IsNullOrWhiteSpace(request.Genre))
        {
            query = query.Where(b => b.BookGenres
                .Any(bg => bg.Genre.Name.ToLower().Contains(request.Genre.ToLower())));
        }

        // 📄 Apply pagination
        var pagedBooks = await query
            .OrderBy(b => b.Id) // Optional: ensure stable ordering
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // ✅ Build response DTOs
        var response = pagedBooks.Select(book => new GetBookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            PublishedDate = book.PublishedDate,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.Name ?? string.Empty,
            Genres = book.BookGenres
                .Where(bg => bg.Genre != null)
                .Select(bg => bg.Genre!.Name)
                .ToList()
        }).ToList();

        return Result.Success(response);
    //}

    //   var books = await _context.Books
    //.Include(b => b.Author)
    //.Include(b => b.BookGenres)
    //    .ThenInclude(bg => bg.Genre)
    //.ToListAsync(cancellationToken);

    //   var response = books.Select(book => new GetBookResponse
    //   {
    //       Id = book.Id,
    //       Title = book.Title,
    //       Description = book.Description,
    //       PublishedDate = book.PublishedDate,
    //       AuthorId = book.AuthorId,
    //       AuthorName = book.Author?.Name ?? string.Empty,
    //       Genres = book.BookGenres
    //           .Where(bg => bg.Genre != null)
    //           .Select(bg => bg.Genre!.Name)
    //           .ToList()
    //   }).ToList();

    //   return Result.Success(response);
}

}

