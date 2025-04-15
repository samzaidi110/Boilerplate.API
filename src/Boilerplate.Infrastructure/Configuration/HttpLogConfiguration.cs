using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boilerplate.Infrastructure.Configuration;

public class HttpLogConfiguration : IEntityTypeConfiguration<HttpLog>
{
    public void Configure(EntityTypeBuilder<HttpLog> builder)
    {
        builder.ToTable("HttpLog", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<DomainId.EfCoreValueConverter>();
    }
}