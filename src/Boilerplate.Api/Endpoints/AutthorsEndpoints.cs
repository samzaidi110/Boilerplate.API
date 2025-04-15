using Boilerplate.Api.Endpoints;
using Ardalis.Result;
using Boilerplate.Application.Features.Books;
using Boilerplate.Application.Features.Books.CreateBook;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using Boilerplate.Application.Auth;
using Boilerplate.Application.Features.Heroes.CreateHero;
using Boilerplate.Domain.Entities.Enums;
using Ardalis.Result.AspNetCore;
using Boilerplate.Api.Policies;
using Boilerplate.Domain.Entities.Common;
using Boilerplate.Application.Features.Books.GetAllBooks;
using System;
using Boilerplate.Application.Features.Books.GetBookById;
using Boilerplate.Application.Features.Books.UpdateBook;
using Boilerplate.Application.Features.Books.DeleteBook;
using Boilerplate.Application.Features.Authors.CreateAuthor;
using Boilerplate.Application.Features.Authors.GetAllAuthors;
using System.Collections.Generic;
using Boilerplate.Application.Features.Authors.GetAuthorById;
using Boilerplate.Application.Features.Authors.GetAuthorsById;

public static class AuthorsEndpoints
{
    public static void MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/authors").WithTags("authors");


        group.MapPost("/", async (
    CreateAuthorRequest request,
    ISender sender) =>
        {
            var result = await sender.Send(request);

            return result.ToMinimalApiResult();
        })
.WithName("CreateAuthor")
.Produces<CreateAuthorResponse>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status409Conflict)
.ProducesProblem(StatusCodes.Status400BadRequest).AllowAnonymous();

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllAuthorsRequest());

            return result.ToMinimalApiResult();
        })
.WithName("GetAllAuthors")
.Produces<List<GetAllAuthorsResponse>>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest).AllowAnonymous();


       

        group.MapGet("/{id:int}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new GetAuthorByIdRequest(id));

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound();
        })
  .WithName("GetAuthorById")
  .Produces<GetAuthorByIdResponse>(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status404NotFound).AllowAnonymous();


   



    }
}
