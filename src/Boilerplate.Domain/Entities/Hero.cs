using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Entities.Interfaces;
using EntityFrameworkCore.EncryptColumn.Attribute;
using MassTransit;

namespace Boilerplate.Domain.Entities;

public class Hero : Entity,IAuditable
{
   
    public string Name { get; set; } = null!;

    public string? Nickname { get; set; }
    public string? Individuality { get; set; } = null!;
    public int? Age { get; set; }

    public HeroType HeroType { get; set; }
    [EncryptColumn]
    public string? Team { get; set; }
}