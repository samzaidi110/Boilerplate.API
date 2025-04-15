using Boilerplate.Api.Policies;
using Boilerplate.Application.Common;
using Boilerplate.Application.Features.Logging;
using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Entities.Logging;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure;
using Boilerplate.Infrastructure.Authorization;
using Boilerplate.Infrastructure.Logging;
using Boilerplate.Infrastructure.MongoDB;
using Boilerplate.Infrastructure.RabbitMq;
using Boilerplate.Infrastructure.Setting;
using Google.Protobuf.WellKnownTypes;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Boilerplate.Api.Configurations;

public static class AuthSetup
{
    public static IServiceCollection AddJwtPolicySetup(this IServiceCollection services)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.PrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        services.AddAuthorizationBuilder()
            .AddRequiredPermissionPolicy();

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = options.GetPolicy(RequiredPermissionDefaults.PolicyName);
        });


        services.AddScoped<IUserAccessor, UserAccessor>();


        return services;
    }
    
    
}