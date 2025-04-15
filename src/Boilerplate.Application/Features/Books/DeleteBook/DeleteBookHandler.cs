using Ardalis.Result;
using Boilerplate.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace Boilerplate.Application.Features.Books.DeleteBook;

public class DeleteBookHandler : IRequestHandler<DeleteBookRequest, Result>
{
    private readonly IContext _context;

    public DeleteBookHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
    {
        var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (book == null)
        {
            return Result.NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
