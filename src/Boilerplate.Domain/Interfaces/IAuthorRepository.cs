using Boilerplate.Domain.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace Boilerplate.Domain.Interfaces;

public interface IAuthorRepository
{
    Task<Author?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Author author);
    public Task SaveChangesAsync();
}
