using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Infrastructure.Postgres;
public class BookDocumentRepository : IBookDocumentRepository
{
    public void Save(Book book)
    {
        Console.WriteLine($"[Persistence] Simulated saving book to a document store: {book.Title}");
    }
}