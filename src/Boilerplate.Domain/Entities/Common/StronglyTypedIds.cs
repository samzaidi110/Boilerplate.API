using StronglyTypedIds;
using System;

[assembly: StronglyTypedIdDefaults(
    backingType: StronglyTypedIdBackingType.Guid,
    converters: StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter |
                StronglyTypedIdConverter.Default | StronglyTypedIdConverter.TypeConverter,
    implementations: StronglyTypedIdImplementations.IEquatable | StronglyTypedIdImplementations.Default)]

namespace Boilerplate.Domain.Entities.Common;


public interface IGuid {}

[StronglyTypedId]
public partial struct DomainId : IGuid
{
    public static implicit operator DomainId(Guid guid)
    {
        return new DomainId(guid);
    }
    
    public static bool TryParse(string? s, IFormatProvider? provider, out DomainId result)
    {
        var parsed = Guid.TryParse(s, provider, out var guid);
        result = guid;
        return parsed;
    }
}

