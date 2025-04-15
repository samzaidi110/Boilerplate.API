using Ardalis.Result;
using Boilerplate.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;


 
namespace Boilerplate.Application.Features.Books.GetAllBooks;

public class GetAllBooksRequest : IRequest<Result<List<GetBookResponse>>>
{
    public string? Genre { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

