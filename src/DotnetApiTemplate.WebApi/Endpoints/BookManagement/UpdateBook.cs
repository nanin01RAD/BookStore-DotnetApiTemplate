using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement;

public class UpdateBook : BaseEndpointWithoutResponse<UpdateBookRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IBookService _bookService;

    public UpdateBook(IDbContext dbContext,
        IBookService bookService)
    {
        _dbContext = dbContext;
        _bookService = bookService;
    }

    [HttpPut("Books/{BookId}")]
    [Authorize]
    [RequiredScope(typeof(BookManagementScope))]
    [SwaggerOperation(
        Summary = "Update Book API",
        Description = "",
        OperationId = "BookManagement.UpdateBook",
        Tags = new[] { "BookManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateBookRequest request,
        CancellationToken cancellationToken = new())
    {
        var Book = await _bookService.GetByExpressionAsync(
            e => e.BookId == request.BookId,
            e => new Book
            {
                Code = e.Code,
                Title = e.Title,
                Author = e.Author,
                Publisher = e.Publisher,
                YearPublish = e.YearPublish,
                Genre = e.Genre,
                Price = e.Price
            },
            cancellationToken);
        if (Book is null)
            return BadRequest(Error.Create("Data not found"));

        _dbContext.AttachEntity(Book);

        if (request.UpdateBookRequestPayload.Code != Book.Code)
            Book.Code = request.UpdateBookRequestPayload.Code;

        if (request.UpdateBookRequestPayload.Title != Book.Title)
            Book.Title = request.UpdateBookRequestPayload.Title;

        if (request.UpdateBookRequestPayload.Author != Book.Author)
            Book.Author = request.UpdateBookRequestPayload.Author;

        if (request.UpdateBookRequestPayload.Publisher != Book.Publisher)
            Book.Publisher = request.UpdateBookRequestPayload.Publisher;

        if (request.UpdateBookRequestPayload.YearPublish != Book.YearPublish)
            Book.YearPublish = request.UpdateBookRequestPayload.YearPublish;

        if (request.UpdateBookRequestPayload.Genre != Book.Genre)
            Book.Genre = request.UpdateBookRequestPayload.Genre;

        if (request.UpdateBookRequestPayload.Description != Book.Description)
            Book.Description = request.UpdateBookRequestPayload.Description;

        if (request.UpdateBookRequestPayload.Price != Book.Price)
            Book.Price = request.UpdateBookRequestPayload.Price;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}