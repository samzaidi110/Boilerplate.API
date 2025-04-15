using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Boilerplate.Application.Features.Identity.GetEmployeeData;

namespace Boilerplate.Api.Endpoints;

public static class HttpServiceEndpoints
{
    public static void MapHttpEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/http")
            .WithTags("http");        
       
        group.MapPost("/sample-http", async (IMediator mediator, GetEmployeeDataRequest request) =>
        {
            var result = await mediator.Send(request);
            return result.ToMinimalApiResult();
        });       
    }
}