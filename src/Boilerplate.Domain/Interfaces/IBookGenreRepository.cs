using Boilerplate.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Interfaces;

public interface IBookGenreRepository
{
    Task<BookGenre?> FindByBookIdAndGenreIdAsync(int bookId, int genreId, CancellationToken cancellationToken = default);

    Task AddAsync(BookGenre bookGenre);
    public Task SaveChangesAsync();

}
