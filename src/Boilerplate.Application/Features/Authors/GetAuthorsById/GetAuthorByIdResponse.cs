using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Authors.GetAuthorsById;

public class GetAuthorByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

