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
using System.Threading;

public static class BooksEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books").WithTags("books");


        group.MapPost("/", async (CreateBookRequest request, ISender sender) =>
        {
            var result = await sender.Send(request);
            return result.Status switch
            {
                ResultStatus.Ok => Results.Created($"/api/books/{result.Value.Id}", "Record Created with id "+ result.Value.Id),
                ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
                ResultStatus.Error => Results.Problem(result.Errors.FirstOrDefault()),
                _ => Results.BadRequest()
            };
        })
        .WithName("CreateBook")
        .WithSummary("Creates a new book.")
        .Produces<GetBookResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError).AllowAnonymous();



        //group.MapGet("/", async (ISender sender) =>
        //{
        //    var result = await sender.Send(new GetAllBooksRequest());

        //    return result.IsSuccess
        //        ? Results.Ok(result.Value)
        //        : Results.Problem(result.Errors.FirstOrDefault());
        //}).AllowAnonymous();

        group.MapGet("/", async (
    [AsParameters] GetAllBooksRequest request,
    ISender mediator,
    CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.ToMinimalApiResult(); // assuming extension to convert Ardalis.Result to IActionResult
        }).AllowAnonymous();

        group.MapGet("/{id:int}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new GetBookByIdRequest(id));

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.NotFound();
        })
  .WithName("GetBookById")
  .Produces<GetBookResponse>(StatusCodes.Status200OK)
  .Produces(StatusCodes.Status404NotFound).AllowAnonymous();


        group.MapPut("/{id:int}", async (int id,UpdateBookRequest request, ISender sender) =>
        {
            request.Id = id;

            var result = await sender.Send(request);

            return result.IsSuccess
                ? Results.Ok("UPDATED")
                : Results.NotFound();
        }).AllowAnonymous();

        group.MapDelete("/{id:int}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBookRequest(id));

            return result.IsSuccess
                ? Results.Ok("Record Deleted with Id "+ id)
                : Results.NotFound();
        }).AllowAnonymous();
    }
}
