using Boilerplate.Application.Common;
using Boilerplate.Application.Features.Logging;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Configuration;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Entities.Interfaces;
using Boilerplate.Domain.Entities.Logging;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure.Authorization;
using Boilerplate.Infrastructure.Configuration;
using EntityFramework.Exceptions.PostgreSQL;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Emit;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure;

public class ApplicationDbContext : DbContext, IContext
{
   private readonly IEncryptionProvider _provider;
    //private  IUserAccessor _userAccessor;
    private readonly IServiceProvider _serviceProvider;
    private const string salt256 = "kljsdkkdlo4454GG00155sajuklmbkdl";
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        //_userAccessor = userAccessor;
        _serviceProvider = serviceProvider;
        this._provider = new GenerateEncryptionProvider(salt256);
        //_userAccessor = userAccessor;
       // _serviceProvider = serviceProvider;
    }

    public DbSet<Hero> Heroes { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<BookGenre> BookGenres { get; set; } = null!;

    public DbSet<HttpLog> HttpLogs { get; set; } = null!;
    public DbSet<ExceptionLog> ExceptionLogs { get; set; } = null!;
    public DbSet<AuditTrail> AuditTrails { get; set; } = null!;
    public DbSet<GlobalSetting> GlobalSetting { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookGenre>()
    .HasKey(bg => new { bg.BookId, bg.GenreId });
        modelBuilder.UseEncryption(this._provider);
        modelBuilder.HasDefaultSchema("public");
        
        modelBuilder.Entity<AuditTrail>().ToTable("AuditTrail", "public");
        modelBuilder.Entity<ExceptionLog>().ToTable("ExceptionLog", "public");
        modelBuilder.Entity<HttpLog>().ToTable("HttpLog", "public");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HttpLogConfiguration).Assembly);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      //  AuditChanges();
        return base.SaveChangesAsync(cancellationToken);
    }
   
    #region Audit

    public  void AuditChanges()
    {
        
        var _userAccessor = _serviceProvider.GetRequiredService<IUserAccessor>();
        var entries =  ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
            .ToList();
        var correlationId = Guid.NewGuid().ToString();
        foreach (var entry in entries)
        {

            var entity = entry.Entity as Entity;

            if (entity != null)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                    entity.Version = 1;
                    entity.ModifiedOn = DateTime.UtcNow;

                    entity.CreatedBy = string.Format("{0}", _userAccessor.UserId);
                    entity.ModifiedBy = string.Format("{0}", _userAccessor.UserId);

                }
                else if (entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                    entity.Version = ++entity.Version;
                    entity.ModifiedBy = string.Format("{0}", _userAccessor.UserId);
                    entity.RowStateId = entry.State == EntityState.Modified && entity.RowStateId != RowState.Deleted ? RowState.Modified : RowState.Deleted;
                }

            }

            if (entry.Entity is IAuditable)
            {
                var auditEntry = CreateAuditEntry(entry);
                auditEntry.CorrelationId = correlationId;
                AuditTrails.Add(auditEntry);
            }
        }

    }

    private static AuditTrail CreateAuditEntry(EntityEntry entry)
    {
        var entity = entry.Entity as Entity;

        var auditEntry = new AuditTrail
        {

            EntityId = entity!.Id, 
            TableName =$"{entry.Metadata.GetTableName()}",
            ActionType = GetActionType(entry),
            Timestamp = DateTimeOffset.UtcNow,
            UserId = null,
            Changes = GetChanges(entry),
            RowStateId = entity.RowStateId,
            Version = entity.Version,
            CreatedBy = entity.CreatedBy, 
            CreatedOn = entity.CreatedOn,
            ModifiedBy = entity.ModifiedBy,
            ModifiedOn = entity.ModifiedOn
        };



        return auditEntry;
    }
    private static string GetActionType(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                return "INSERT";
            case EntityState.Modified:
                return "UPDATE";
            case EntityState.Deleted:
                return "DELETE";
            default:
                return "NA";
        }
    }

    private static string GetChanges(EntityEntry entry)
    {
        var changes = new List<object>();

        foreach (var property in entry.OriginalValues.Properties)
        {
            var original = entry.OriginalValues[property];
            var current = entry.CurrentValues[property];

            if (!object.Equals(original, current))
            {
                changes.Add(new
                {
                    PropertyName = property.Name,
                    OriginalValue = original,
                    CurrentValue = current
                });
            }
        }

        return JsonSerializer.Serialize(changes);
    }
    #endregion

}