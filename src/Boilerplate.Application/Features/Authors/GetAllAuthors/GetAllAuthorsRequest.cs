using Ardalis.Result;
using MediatR;
using System.Collections.Generic;

namespace Boilerplate.Application.Features.Authors.GetAllAuthors;

public class GetAllAuthorsRequest : IRequest<Result<List<GetAllAuthorsResponse>>>
{
}
