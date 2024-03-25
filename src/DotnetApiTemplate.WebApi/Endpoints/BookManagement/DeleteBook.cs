using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement;

public class DeleteBook : BaseEndpointWithoutResponse<DeleteBookRequest>
{
    private readonly IBookService _bookService;
    private readonly IDbContext _dbContext;

    public DeleteBook(IBookService bookService,
        IDbContext dbContext)
    {
        _bookService = bookService;
        _dbContext = dbContext;
    }

    [HttpDelete("Book/{BookId}")]
    [Authorize]
    [RequiredScope(typeof(BookManagementScope))]
    [SwaggerOperation(
        Summary = "Delete Book API",
        Description = "",
        OperationId = "BookManagement.DeleteBook",
        Tags = new[] { "BookManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteBookRequest request,
        CancellationToken cancellationToken = new())
    {
        var Book = await _bookService.GetByIdAsync(request.BookId, cancellationToken);
        if (Book is null)
            return BadRequest(Error.Create("Order data not found"));

        var BookOrder = await _bookService.IsBookOrder(request.BookId, cancellationToken);

        if (BookOrder.Count() > 0)
            return BadRequest(Error.Create("The Book has been order processed"));

        await _bookService.DeleteAsync(request.BookId, cancellationToken);

        return NoContent();
    }
}