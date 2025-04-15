using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Net;

namespace Boilerplate.Api.Endpoints;


public static class EchoEndpoints
{
    public class CustomResponse
    {
        public int StatusCode { get; set; }
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString();
        public string ApiName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public static void MapEchoEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/_echo")
                .WithTags("_echo");

        // Define the echo endpoint
        group.MapGet("/",  (IMediator mediator) =>
        {
            var response = new CustomResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Timestamp = DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), // Add current timestamp
                ApiName = "_EchoAPI", // Add your API name here
                Description = "Echo API is designed to check the deployment process without any Parameter and Authorisation." // Add your API name here
            };

            return response;
        });
    }
}