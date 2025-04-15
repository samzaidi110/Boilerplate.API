using Ardalis.Result;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Domain.Entities.Enums;
using MediatR;
using System;
using System.Text.Json.Serialization;

namespace Boilerplate.Application.Features.Logging.UpdateHttpLog;

public record UpdateHttpLogRequest : IRequest<Result<VoidResponse>>
{
    [JsonIgnore]
    public DomainId Id { get; init; }
    public string ResponseBody { get; set; } = string.Empty;
    public int Status { get; set; }
    public int DurationMilliseconds { get; set; }
    public string Type { get; set; } = string.Empty;

}