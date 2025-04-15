using Ardalis.Result;
using MediatR;

namespace Boilerplate.Application.Features.Authors.CreateAuthor;

public class CreateAuthorRequest : IRequest<Result<CreateAuthorResponse>>
{
    public string Name { get; set; } = string.Empty;
}
