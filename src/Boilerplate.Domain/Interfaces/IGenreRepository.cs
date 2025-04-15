using Boilerplate.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Interfaces;

public interface IGenreRepository
{
    Task<Genre?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Genre genre);
    public Task SaveChangesAsync();
}
