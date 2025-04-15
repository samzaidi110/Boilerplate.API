using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Domain.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Logging.CreateHttpLog;

public class CreateHttpLogHandler : IRequestHandler<CreateHttpLogRequest, Result<VoidResponse>>
{
    private readonly IContext _context;
    private readonly IOperationScoped _operationScoped;

    public CreateHttpLogHandler(IContext context, IOperationScoped operationScoped)
    {
        _context = context;
        _operationScoped = operationScoped;
    }

    public async Task<Result<VoidResponse>> Handle(CreateHttpLogRequest request, CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Logging.HttpLog>();
        created.CorrelationId = _operationScoped.OperationId;
        _context.HttpLogs.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<VoidResponse>();
    }
}