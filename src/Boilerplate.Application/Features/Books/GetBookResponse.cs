using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Books;
public class GetBookResponse
{

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime PublishedDate { get; set; }

    public string AuthorName { get; set; } = string.Empty;
    public int AuthorId { get; set; }

    //public List<string> Genres { get; set; } = new();

    //public int Id { get; set; }
    //public string Title { get; set; } = string.Empty;
    //public string? Description { get; set; }
    //public DateTime PublishedDate { get; set; }
    //public string AuthorName { get; set; } = string.Empty;
    //public int AuthorId { get; set; }
    //public Author Author { get; set; } = null!;
    public List<string> Genres { get; set; } = new();
 

    //public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

}

 
