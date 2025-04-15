using Ardalis.Result.AspNetCore;
using Boilerplate.Api.Policies;
using Boilerplate.Application.Auth;
using Boilerplate.Application.Features.Heroes.CreateHero;
using Boilerplate.Application.Features.Heroes.DeleteHero;
using Boilerplate.Application.Features.Heroes.GetAllHeroes;
using Boilerplate.Application.Features.Heroes.GetHeroById;
using Boilerplate.Application.Features.Heroes.UpdateHero;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security;

namespace Boilerplate.Api.Endpoints;

public static class HeroEndpoints
{
    public static void MapHeroEndpoints(this IEndpointRouteBuilder builder)
    {
      
        var group = builder.MapGroup("api/Hero")
            .WithTags("Hero");
        
        group.MapGet("/", async (IMediator mediator, [AsParameters] GetAllHeroesRequest request) =>
        {
            var result = await mediator.Send(request);
            return result;
        }).RequirePermission(new PermissionRequirement { BusinessDomain = "Client", Permission = Permissions.CanView }); 

        group.MapGet("{id}", async (IMediator mediator, DomainId id) =>
        {
            var result = await mediator.Send(new GetHeroByIdRequest(id));
            return result.ToMinimalApiResult();
        });

        group.MapPost("/", async (IMediator mediator, CreateHeroRequest request) =>
        {
            var result = await mediator.Send(request);
            return result.ToMinimalApiResult();
        }).RequirePermission(new PermissionRequirement{BusinessDomain = "Client", Permission = Permissions.CanAdd }); 

        group.MapPut("{id}", async (IMediator mediator, DomainId id, UpdateHeroRequest request) =>
        {
            var result = await mediator.Send(request with { Id = id });
            return result.ToMinimalApiResult();
        }).AllowAnonymous();

        group.MapDelete("{id}", async (IMediator mediator, DomainId id) =>
        {
            var result = await mediator.Send(new DeleteHeroRequest(id));
            return result.ToMinimalApiResult();
        });
    }
}