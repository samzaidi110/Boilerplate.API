using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Heroes.UpdateHero;

public class UpdateHeroHandler : IRequestHandler<UpdateHeroRequest, Result<GetHeroResponse>>
{
    private readonly IContext _context;
    private readonly IUserAccessor _userAccessor;
    public UpdateHeroHandler(IContext context, IUserAccessor userAccessor)
    {
        _context = context;
        _userAccessor= userAccessor;
    }

    public async Task<Result<GetHeroResponse>> Handle(UpdateHeroRequest request,
        CancellationToken cancellationToken)
    {

        if (_userAccessor.IsAuthenticated && _userAccessor.HasPermission("Client", Permissions.CanModify))
        {

            var originalHero = await _context.Heroes
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (originalHero == null) return Result.NotFound();

            originalHero.Name = request.Name;
            originalHero.Nickname = request.Nickname;
            originalHero.Team = request.Team;
            originalHero.Individuality = request.Individuality;
            originalHero.Age = request.Age;
            originalHero.HeroType = request.HeroType;
            _context.Heroes.Update(originalHero);
            await _context.SaveChangesAsync(cancellationToken);
            return originalHero.Adapt<GetHeroResponse>();
        }
        else
        {
            return Result.Forbidden();
        }
    }
}