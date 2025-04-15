using Boilerplate.Domain.Entities.Common;
using System;

namespace Boilerplate.Domain.Entities.Logging;
public class HttpLog : Entity
{
    public string CorrelationId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Headers { get; set; } = string.Empty;
    public string RequestBody { get; set; } = string.Empty;
    public string? ResponseBody { get; set; } = string.Empty;
    public string RequesterIpAddress { get; set; } = string.Empty;
    public int Status { get; set; }
    public int DurationMilliseconds { get; set; }
    public string Type { get; set; } = string.Empty;

}