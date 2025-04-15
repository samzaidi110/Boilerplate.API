using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Domain.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Logging.CreateExceptionLog;

public class CreateExceptionLogHandler : IRequestHandler<CreateExceptionLogRequest, Result<VoidResponse>>
{
    private readonly IContext _context;
    private readonly IOperationScoped _operationScoped;


    public CreateExceptionLogHandler(IContext context, IOperationScoped operationScoped)
    {
        _context = context;
        _operationScoped = operationScoped;
    }

    public async Task<Result<VoidResponse>> Handle(CreateExceptionLogRequest request, CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Logging.ExceptionLog>();
        created.CorrelationId = _operationScoped.OperationId;
        _context.ExceptionLogs.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<VoidResponse>();
    }
}