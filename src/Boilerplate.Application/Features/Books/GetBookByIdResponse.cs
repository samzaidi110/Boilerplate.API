using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Books;
public class GetBookByIdResponse
{

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? AuthorName { get; set; }

    public List<string> Genres { get; set; } = new();
     

}

 
