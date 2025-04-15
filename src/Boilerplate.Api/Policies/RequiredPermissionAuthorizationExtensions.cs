using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Boilerplate.Api.Policies;

public static class RequiredPermissionDefaults
{
    public const string PolicyName = "RequiredPermission";
}

public static class RequiredPermissionAuthorizationExtensions
{
    public static AuthorizationPolicyBuilder RequireRequiredPermissions(this AuthorizationPolicyBuilder builder)
    {
        return builder.AddRequirements(new PermissionAuthorizationRequirement());
    }

    public static AuthorizationBuilder AddRequiredPermissionPolicy(this AuthorizationBuilder builder)
    {
        builder.AddPolicy(RequiredPermissionDefaults.PolicyName, policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);

            policy.RequireRequiredPermissions();
        });

    
        builder.Services.AddScoped<IAuthorizationHandler, RequiredPermissionAuthorizationHandler>();

        return builder;
    }
}