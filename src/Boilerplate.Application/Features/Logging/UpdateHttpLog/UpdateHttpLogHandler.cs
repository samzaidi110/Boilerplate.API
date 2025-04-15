using Ardalis.Result;
using Boilerplate.Application.Common;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Logging.UpdateHttpLog;

public class UpdateHttpLogHandler : IRequestHandler<UpdateHttpLogRequest, Result<VoidResponse>>
{
    private readonly IContext _context;
    
    
    public UpdateHttpLogHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<VoidResponse>> Handle(UpdateHttpLogRequest request, CancellationToken cancellationToken)
    {
       
        var httpLog=  _context.HttpLogs.FirstOrDefault(x => x.Id == request.Id);
        if (httpLog != null)
        {
            httpLog.ResponseBody = request.ResponseBody;
            httpLog.DurationMilliseconds = request.DurationMilliseconds;
            httpLog.Status = request.Status;
            httpLog.Type = request.Type;
            _context.HttpLogs.Update(httpLog);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return httpLog.Adapt<VoidResponse>();
    }
}