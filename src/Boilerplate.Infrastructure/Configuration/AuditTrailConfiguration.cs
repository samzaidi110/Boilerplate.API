using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boilerplate.Infrastructure.Configuration;

public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
{
    public void Configure(EntityTypeBuilder<AuditTrail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("AuditTrail", "public");
        builder.Property(x => x.Id)
            .HasConversion<DomainId.EfCoreValueConverter>();
        builder.Property(x => x.EntityId)
           .HasConversion<DomainId.EfCoreValueConverter>();
    }
}