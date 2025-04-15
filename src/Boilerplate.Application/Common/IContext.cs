using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Configuration;
using Boilerplate.Domain.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Common;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DatabaseFacade Database { get; }
    
    public DbSet<Hero> Heroes { get; }
    public DbSet<Book> Books { get; }
    public DbSet<BookGenre> BookGenres { get; }
    public DbSet<Author> Authors { get; }
    public DbSet<Genre> Genres { get; }

    public DbSet<HttpLog> HttpLogs { get; }
    public DbSet<ExceptionLog> ExceptionLogs { get; }
    public DbSet<AuditTrail> AuditTrails { get; }
    public DbSet<GlobalSetting> GlobalSetting { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}