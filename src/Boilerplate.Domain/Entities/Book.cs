using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities;
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime PublishedDate { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}

