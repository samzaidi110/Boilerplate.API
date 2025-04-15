using Ardalis.Result;
using Boilerplate.Application.Common;
using Boilerplate.Domain.Interfaces;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Heroes.CreateHero;

public class CreateHeroHandler : IRequestHandler<CreateHeroRequest, Result<GetHeroResponse>>
{
    private readonly IContext _context;
    private readonly IHeroDocumentRepository _heroDocumentRepository;
    private readonly IRabbitMQService _rabbitMQService;
    
    public CreateHeroHandler(IContext context, IHeroDocumentRepository heroDocumentRepository, IRabbitMQService rabbitMQService)
    {
        _context = context;
        _heroDocumentRepository = heroDocumentRepository;
        _rabbitMQService = rabbitMQService;
    }

    public async Task<Result<GetHeroResponse>> Handle(CreateHeroRequest request, CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Hero>();

        if (created.Age >= 70) {
            throw new BusinessRuleException(300, "Hero is too old.");
        }
        _context.Heroes.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        _rabbitMQService.PushToHeroQueue(created);
        _heroDocumentRepository.Save(created);
        return created.Adapt<GetHeroResponse>();
    }
}