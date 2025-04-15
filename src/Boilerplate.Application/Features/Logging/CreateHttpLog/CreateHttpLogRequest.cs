using Ardalis.Result;
using Boilerplate.Domain.Entities.Enums;
using MediatR;
using System;

namespace Boilerplate.Application.Features.Logging.CreateHttpLog;

public record CreateHttpLogRequest : IRequest<Result<VoidResponse>>
{
    public string CorrelationId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Headers { get; set; } = string.Empty;
    public string RequestBody { get; set; } = string.Empty;
    public string RequesterIpAddress { get; set; } = string.Empty;
    public int Status { get; set; }
    public int DurationMilliseconds { get; set; }
    public string Type { get; set; } = string.Empty;
}