using Ardalis.Result;
using Boilerplate.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using System.Collections.Generic;

namespace Boilerplate.Application.Features.Books.UpdateBook;

public class UpdateBookHandler : IRequestHandler<UpdateBookRequest, Result>
{
    private readonly IContext _context;
    private readonly IBookDocumentRepository _bookDocumentRepository;

    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IBookGenreRepository _bookGenreRepository;

    public UpdateBookHandler(IContext context, IBookDocumentRepository bookDocumentRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, IBookGenreRepository bookGenreRepository)
    {
        _context = context;
        _bookDocumentRepository = bookDocumentRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _bookGenreRepository = bookGenreRepository;
    }

    public async Task<Result> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
        if (book == null)
        {
            return Result.NotFound();
        }

        var author = await _authorRepository.GetByNameAsync(request.AuthorName.ToLower(), cancellationToken);

        if (author == null)
        {
            author = new Author { Name = request.AuthorName };
            await _authorRepository.AddAsync(author);
            await _authorRepository.SaveChangesAsync();
        }
        else
        {
            request.AuthorId = author.Id;
        }

        // Get or create genres
        var bookGenres = new List<BookGenre>();
        foreach (var genreName in request.Genres)
        {
            var genre = await _genreRepository.GetByNameAsync(genreName.ToLower(), cancellationToken);

            if (genre == null)
            {
                genre = new Genre { Name = genreName };
                await _genreRepository.AddAsync(genre);
                await _genreRepository.SaveChangesAsync(); // Ensure Genre.Id is populated
            }

            bookGenres.Add(new BookGenre { GenreId = genre.Id });
        }
       

        book.Id = request.Id;
        book.Title = request.Title;
        book.PublishedDate = request.PublishedDate;
        book.AuthorId = request.AuthorId;
        book.Description = request.Description;
        book.BookGenres = bookGenres;

        //var createdBook = await _context.Books
        //    .Include(b => b.Author)
        //    .Include(b => b.BookGenres)
        //        .ThenInclude(bg => bg.Genre)
        //    .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);


        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
