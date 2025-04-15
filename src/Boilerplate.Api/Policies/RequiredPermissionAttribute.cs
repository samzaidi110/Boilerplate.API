using Boilerplate.Application.Auth;
using Boilerplate.Domain.Entities.Enums;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System;
using System.Security;

namespace Boilerplate.Api.Policies;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiredPermissionAttribute : Attribute, IRequiredPermissionMetadata
{
    public HashSet<PermissionRequirement> RequiredPermissions { get; }

    public RequiredPermissionAttribute(PermissionRequirement requiredPermission, params PermissionRequirement[] additionalRequiredPermissions)
    {
        RequiredPermissions = new HashSet<PermissionRequirement>();
        RequiredPermissions.Add(requiredPermission);
        foreach (var permission in additionalRequiredPermissions)
        {
            RequiredPermissions.Add(permission);
        }
    }
}



public static class AuthorizationExtensions
{
    public static TBuilder RequirePermission<TBuilder>(
        this TBuilder endpointConventionBuilder, PermissionRequirement requiredPermission, params PermissionRequirement[] additionalRequiredPermissions)
        where TBuilder : IEndpointConventionBuilder
    {
        return endpointConventionBuilder.WithMetadata(new RequiredPermissionAttribute(requiredPermission, additionalRequiredPermissions));
    }
}