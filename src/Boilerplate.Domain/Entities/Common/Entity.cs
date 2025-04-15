using Boilerplate.Domain.Entities.Enums;
using MassTransit;
using System;

namespace Boilerplate.Domain.Entities.Common;


public abstract class BaseEntity
{
    public RowState RowStateId { get; set; } = RowState.Created;
    public int Version { get; set; } = 1;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string ModifiedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOn { get; set; }
}


public abstract class Entity : BaseEntity
{
    public virtual DomainId Id { get; set; } = NewId.NextGuid();
}



public abstract class LookupEntity : BaseEntity
{
    public virtual int Id { get; set; }
    public required  virtual string Description { get; set; }
}
