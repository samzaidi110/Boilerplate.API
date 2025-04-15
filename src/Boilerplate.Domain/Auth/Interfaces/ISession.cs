using Boilerplate.Domain.Entities.Common;
using System;

namespace Boilerplate.Domain.Auth.Interfaces;

public interface ISession
{
    public DomainId UserId { get; }

    public DateTime Now { get; }
}