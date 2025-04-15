//using Ardalis.Result;
//using Boilerplate.Application.Common;
//using Boilerplate.Domain.Entities;
//using Boilerplate.Domain.Interfaces;
//using Mapster;
//using MediatR;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Boilerplate.Application.Features.Books.CreateBook;

//public class CreateBookCommandHandler : IRequestHandler<CreateBookRequest, Result<GetBookResponse>>
//{
//    private readonly IContext _context;
//    private readonly IBookDocumentRepository _bookDocumentRepository;
//    private readonly IRabbitMQService _rabbitMQService;

//    public CreateBookCommandHandler(IContext context, IBookDocumentRepository bookDocumentRepository, IRabbitMQService rabbitMQService)
//    {
//        _context = context;
//        _bookDocumentRepository = bookDocumentRepository;

//        _rabbitMQService = rabbitMQService;

//    }

//    public async Task<Result<GetBookResponse>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
//    {
//        var book = request.Adapt<Book>();

//        // Domain business rule
//        if (book.PublishedDate > DateTime.UtcNow)
//        {
//            throw new BusinessRuleException(301, "Book cannot be published in the future.");
//        }

//        _context.Books.Add(book);
//        await _context.SaveChangesAsync(cancellationToken);

//        //_rabbitMQService.PushToBookQueue(book);
//        _bookDocumentRepository.Save(book);

//        return book.Adapt<GetBookResponse>();
//    }
//}

//will check
using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;

namespace Boilerplate.Application.Features.Books.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookRequest, Result<GetBookResponse>>
{
    private readonly IContext _context;
    private readonly IBookDocumentRepository _bookDocumentRepository;

    private readonly IAuthorRepository _authorRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IBookGenreRepository _bookGenreRepository;


    public CreateBookCommandHandler(IContext context, IBookDocumentRepository bookDocumentRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, IBookGenreRepository bookGenreRepository)
    {
        _context = context;
        _bookDocumentRepository = bookDocumentRepository;
        _authorRepository = authorRepository;
        _genreRepository = genreRepository;
        _bookGenreRepository = bookGenreRepository;
    }

    public async Task<Result<GetBookResponse>> Handle(CreateBookRequest request, CancellationToken cancellationToken)
    {
        if (request.PublishedDate > DateTime.UtcNow)
        {
            throw new BusinessRuleException(301, "Book cannot be published in the future.");
        }

        var book = request.Adapt<Book>();

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

        var bookc = new Book
        {
            Title = request.Title,
            Description = request.Description,
            PublishedDate = request.PublishedDate,
            AuthorId = author.Id,
            BookGenres = bookGenres
        };

        _context.Books.Add(bookc);
        await _context.SaveChangesAsync();

        _bookDocumentRepository.Save(bookc);


        // Reload the book with related data
        var createdBook = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.BookGenres)
                .ThenInclude(bg => bg.Genre)
            .FirstOrDefaultAsync(b => b.Id == bookc.Id, cancellationToken);

        if (createdBook.Id != null)
        {



            foreach (var bookgenera in bookGenres)
            {
                var bookgeneres = await _bookGenreRepository.FindByBookIdAndGenreIdAsync(createdBook.Id, bookgenera.GenreId, cancellationToken);

                if (bookgeneres == null)
                {
                    bookgeneres = new BookGenre { BookId = createdBook.Id, GenreId = bookgenera.GenreId };
                    await _bookGenreRepository.AddAsync(bookgeneres);
                    await _bookGenreRepository.SaveChangesAsync(); // Ensure Genre.Id is populated
                }
            }
        }
        var response = new GetBookResponse
        {
            Id = createdBook.Id,
            Title = createdBook.Title,
            Description = createdBook.Description,
            AuthorName = createdBook.Author.Name,
            Genres = createdBook.BookGenres.Select(bg => bg.Genre.Name).ToList()
        };

        return response;
    }
}




