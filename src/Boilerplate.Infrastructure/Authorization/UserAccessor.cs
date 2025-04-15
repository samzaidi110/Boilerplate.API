using Amazon.Runtime.Internal;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Authorization;
public class UserAccessor : IUserAccessor
{
    public DomainId UserId { get; private init; }

    public bool IsAuthenticated { get; private init; }

    public AuthUser User { get; private init; }

    public DateTime Now => DateTime.Now;

    public List<int> Groups { get; private init; }

    public List<int> ProfileTypes { get; private init; } = new AutoConstructedList<int>();

    private Dictionary<string, string> Granted { get; init; } = new Dictionary<string, string>();
    private Dictionary<string, string> Override { get; init; } = new Dictionary<string, string>();

    public UserAccessor(IHttpContextAccessor httpContextAccessor) : this(httpContextAccessor?.HttpContext?.User)
    {
    }

    public UserAccessor(ClaimsPrincipal? user)
    {
        IsAuthenticated = !(user == null ||
                            user.Identity?.IsAuthenticated == false);

        Groups = new List<int>();
        User = new AuthUser();
        if (IsAuthenticated && user != null)
        {
            UserId = new Guid(user.FindFirst("id")!.Value);

            User = new AuthUser
            {
                Id = new Guid(SafeClaim("id")),
                Email = SafeClaim(ClaimTypes.Email),
                ShortName = SafeClaim(ClaimTypes.GivenName),
                UserName = SafeClaim(ClaimTypes.Name),
                Mobile = SafeClaim(ClaimTypes.MobilePhone)
            };
            Groups = JsonConvert.DeserializeObject<List<int>>(SafeClaim("Groups")) ?? new List<int>();
            ProfileTypes = JsonConvert.DeserializeObject<List<int>>(SafeClaim("Types")) ?? new List<int>();
            Granted = JsonConvert.DeserializeObject<Dictionary<string, string>>(SafeClaim("GrantedPermissions")) ?? new Dictionary<string, string>();

            Override = JsonConvert.DeserializeObject<Dictionary<string, string>>(SafeClaim("OverridePermissions")) ?? new Dictionary<string, string>();

        }

        string SafeClaim(string claimName)
        {
            return $"{user?.FindFirst(claimName)?.Value}" ;
        }
    }

    public bool HasPermission(string businessDomain, Permissions permission)
    {
      
        if (Granted.TryGetValue(businessDomain, out string? permissionSet))
        {
            return (Convert.ToInt32(permissionSet, 16) & (int)permission) != 0;

        }

        return false;

    }


}
