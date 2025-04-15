using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Interfaces;

public interface IUserAccessor
{
    public bool IsAuthenticated { get; }
    public DomainId UserId { get; }

    public List<int> Groups { get; }

    public List<int> ProfileTypes { get; }
    public AuthUser User { get; }
    public DateTime Now { get; }

    bool HasPermission(string businessDomain, Permissions permission);
}