using Boilerplate.Application.Auth;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Boilerplate.Api.Policies;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement;

public sealed class RequiredPermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{

    private readonly IUserAccessor _session;
    public RequiredPermissionAuthorizationHandler(IUserAccessor session)
    {
        _session = session;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,  PermissionAuthorizationRequirement requirement)
    {


    

    var endpoint = context.Resource switch
        {
            HttpContext httpContext => httpContext.GetEndpoint(),
            Endpoint ep => ep,
            _ => null,
        };

        var requiredPermissions = endpoint?.Metadata.GetMetadata<IRequiredPermissionMetadata>()?.RequiredPermissions;
        if (requiredPermissions == null)
        {
            // The endpoint is not decorated with the required permission metadata
            context.Succeed(requirement);
            return Task.CompletedTask;
        }


        if (!_session.IsAuthenticated)
        {
            context.Fail(); // No HTTP context available, fail the requirement
            return Task.CompletedTask;
        }

        var hasRequiredPermissions = false;
        foreach (var requiredPermission in requiredPermissions)
        {
            hasRequiredPermissions = hasRequiredPermissions || requiredPermission.BusinessDomain=="None" || _session.HasPermission(requiredPermission.BusinessDomain, requiredPermission.Permission);
        }

  
        if (hasRequiredPermissions)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
