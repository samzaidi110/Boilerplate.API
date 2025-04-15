using Boilerplate.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities.Logging;
public class ExceptionLog : Entity
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
    public string CorrelationId { get; set; }= string.Empty;


}
