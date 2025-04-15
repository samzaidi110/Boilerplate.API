using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using MediatR;
using Boilerplate.Application.Features.Logging.CreateHttpLog;
using Boilerplate.Application.Features.Logging.UpdateHttpLog;

namespace Boilerplate.Infrastructure.Logging;
public class OutgoingLoggingHandler : DelegatingHandler
{
    private readonly IOperationScoped _operationScoped;
    private readonly IMediator _mediator;

    public OutgoingLoggingHandler(IOperationScoped operationScoped, IMediator mediator)
    {
        _operationScoped = operationScoped;
        _mediator = mediator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var timestamp = DateTime.UtcNow;
        request.Headers.Add("X-CORRELATION-ID", _operationScoped.OperationId);

        //Process Log
        var httpRequestLog = new CreateHttpLogRequest
        {
            CorrelationId = _operationScoped.OperationId,
            Timestamp = DateTime.UtcNow,
            HttpMethod = request.Method.ToString(),
            Url = $"{request.RequestUri}",
            Headers = FormatHeaders(request),
            RequestBody = request.Content == null ? "" : await request.Content.ReadAsStringAsync(CancellationToken.None),
            RequesterIpAddress = "out-going",
            Status = 0, // Status will be updated later
            DurationMilliseconds = 0,// Duration will be updated later
            Type = "OutRequest"
        };

        // Log HTTP request
        var reqLog = await _mediator.Send(httpRequestLog,CancellationToken.None);

        var response = await base.SendAsync(request, cancellationToken);


        var httpResponseLog = new UpdateHttpLogRequest
        {
            Id = reqLog.Value.Id,
            ResponseBody = await response.Content.ReadAsStringAsync(CancellationToken.None),
            Status = (int)response.StatusCode,
            DurationMilliseconds = (int)(DateTime.UtcNow - timestamp).TotalMilliseconds,
            Type = "OutResponse"
        };

        await _mediator.Send(httpResponseLog,CancellationToken.None);

        return response;
    }

    private static string FormatHeaders(HttpRequestMessage request)
    {
        StringBuilder headerString = new StringBuilder();

        foreach (var header in request.Headers)
        {
            headerString.Append($"{header.Key}: {string.Join(", ", header.Value)}{Environment.NewLine}");

        }

        return headerString.ToString();
    }
}
