using Ardalis.Result;
using Boilerplate.Domain.Entities.Enums;
using MediatR;
using System;

namespace Boilerplate.Application.Features.Logging.CreateExceptionLog;

public record CreateExceptionLogRequest : IRequest<Result<VoidResponse>>
{
    public string IncidentNumber { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string ExceptionType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Headers { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string RequesterIpAddress { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
}