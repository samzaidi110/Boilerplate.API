using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Boilerplate.Infrastructure.Postgres;
public class BookGenreRepository : IBookGenreRepository
{
    private readonly ApplicationDbContext _context;

    public BookGenreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookGenre?> FindByBookIdAndGenreIdAsync(int bookId, int genreId, CancellationToken cancellationToken = default)
    {
        return await _context.BookGenres
     .FirstOrDefaultAsync(bg => bg.BookId == bookId && bg.GenreId == genreId, cancellationToken);
    }

    public async Task AddAsync(BookGenre bookGenres)
    {
        await _context.BookGenres.AddAsync(bookGenres);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
