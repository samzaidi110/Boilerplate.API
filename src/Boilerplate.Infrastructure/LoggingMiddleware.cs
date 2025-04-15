// Infrastructure Layer

using Boilerplate.Domain.Entities.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using MediatR;
using Azure.Core;
using System.Linq;
using System.IO;
using System.Text;
using Boilerplate.Application.Common;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Internal;
using Boilerplate.Application.Features.Logging.UpdateHttpLog;
using Boilerplate.Application.Features.Logging.CreateExceptionLog;
using System.Text.Json;
using System.Collections.Generic;
using Boilerplate.Domain.Interfaces;
using Boilerplate.Infrastructure.Logging;
using Amazon.Runtime.Internal;
using System.Security.Cryptography;

namespace Boilerplate.Infrastructure;
public class LoggingMiddleware : IMiddleware
{

    private readonly IMediator _mediator;
    private readonly IOperationScoped _operationScoped;
    public LoggingMiddleware(IOperationScoped operationScoped, IMediator mediator)
    {
        _operationScoped = operationScoped;
        _mediator = mediator;
      
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = _operationScoped.OperationId;

        var timestamp = DateTime.UtcNow;

        var originalResponseStream = context.Response.Body;
      
       
        context.Request.EnableBuffering();
        context.Response.Headers.Add("X-CORRELATION-ID", correlationId);
        var httpRequestLog = new Boilerplate.Application.Features.Logging.CreateHttpLog.CreateHttpLogRequest
        {
            CorrelationId = correlationId,
            Timestamp = timestamp,
            HttpMethod = context.Request.Method,
            Url = context.Request.Path,
            Headers =  FormatHeaders(context.Request),
            RequestBody = await FormatRequestBody(context.Request),
            RequesterIpAddress = $"{context.Connection.RemoteIpAddress}" ,
            Status = 0, // Status will be updated later
            DurationMilliseconds = 0,// Duration will be updated later
            Type = "Request"
        };

        try
        {
            // Log HTTP request
          
            var request = await _mediator.Send(httpRequestLog);
            // Continue processing the request
            UpdateHttpLogRequest httpResponseLog = new UpdateHttpLogRequest{};
            using (var memoryBody = new MemoryStream())
            {
                context.Response.Body = memoryBody;

                await next(context);

                // Log HTTP response

                httpResponseLog = new Boilerplate.Application.Features.Logging.UpdateHttpLog.UpdateHttpLogRequest
                {
                    Id = request.Value.Id,
                    ResponseBody = await FormatResponseBody(context.Response),
                    Status = context.Response.StatusCode,
                    DurationMilliseconds = (int)(DateTime.UtcNow - timestamp).TotalMilliseconds,
                    Type = "Response"
                };

                await memoryBody.CopyToAsync(originalResponseStream);
            }

            await _mediator.Send(httpResponseLog);
        }
        catch (BusinessRuleException businessRuleException)
        {
            var incidentResponse = new List<ValidationError>
            {
                new ValidationError($"{businessRuleException.ResponseCode}",businessRuleException.Message,$"{businessRuleException.ResponseCode}", 0)
            };

            var responseJson = JsonSerializer.Serialize(incidentResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;

            await originalResponseStream.WriteAsync(Encoding.UTF8.GetBytes(responseJson));
        }
        catch (Exception ex)
        {
            var incidentNumber = GenerateUniqueIncidentNumber();
            CreateExceptionLogRequest createExceptionLogRequest = new CreateExceptionLogRequest
            {
                Body = string.IsNullOrEmpty($"{httpRequestLog.RequestBody}") ? await FormatRequestBody(context.Request) : httpRequestLog.RequestBody,
                CorrelationId = correlationId,
                ExceptionType = ex.GetType().Name,
                Headers =  FormatHeaders(context.Request),
                HttpMethod = context.Request.Method,
                Message = ex.Message,
                Timestamp = DateTime.UtcNow,
                Url = context.Request.Path,
                RequesterIpAddress =$"{context!.Connection?.RemoteIpAddress?.ToString()}",
                StackTrace = $"{ex.StackTrace}",
                IncidentNumber = incidentNumber

            };
            await _mediator.Send(createExceptionLogRequest);

            var incidentResponse = new
            {
                Exception = new
                {
                    Message = "We're sorry, but something unexpected happened on our end.",
                    IncidentNumber = incidentNumber,
                    suggestedAction = "Please try again later or contact support if the issue persists.",
                    Timestamp = DateTime.UtcNow

                }
            };

            var responseJson = JsonSerializer.Serialize(incidentResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500; 
        
            await originalResponseStream.WriteAsync(Encoding.UTF8.GetBytes(responseJson));

            // throw; // Re-throw the exception for further handling
        }
    }
    private static string GenerateUniqueIncidentNumber()
    {
        
    var randomGenerator = RandomNumberGenerator.Create(); // Compliant for security-sensitive use cases
        byte[] data = new byte[16];
        randomGenerator.GetBytes(data);
        var randomFactor= BitConverter.ToString(data);

        return  $"SOLV-EX-{DateTime.Now.ToString("yyyyMMddHHmmss")}{randomFactor}";
    }
    private static async Task<string> FormatRequestBody(HttpRequest request)
    {
        if (request.ContentType != null &&
            !request.ContentType.Contains("multipart/form-data"))
        {
            var bodyReader = new StreamReader(request.Body);
            
                var body = await bodyReader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            
        }
        else
        {
            return "multipart/form-data"; //TODO: Log Form Fields
        }
    }

    private static async Task<string> FormatResponseBody(HttpResponse response)
    {
        var body = "Response contains File";
        response.Body.Position = 0;
        if (!response.Headers.ContainsKey("Content-Disposition"))
        {

            body = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Position = 0;
        }

        return body;
    }
    private static string FormatHeaders(HttpRequest request)
    {
        StringBuilder headerString = new StringBuilder();

        foreach (var header in request.Headers)
        {
            headerString.Append($"{header.Key}: {string.Join(", ", $"{header!.Value}")}{Environment.NewLine}");

        }
        return headerString.ToString();

    }


}
