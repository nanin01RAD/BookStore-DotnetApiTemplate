using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Queries;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Responses;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.BookManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace DotnetApiTemplate.WebApi.Endpoints.BookManagement;

public class GetAllBookPaginated : BaseEndpoint<GetAllBookPaginatedRequest, PagedList<BookResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllBookPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("Books")]
    [Authorize]
    [RequiredScope(typeof(BookManagementScope), typeof(BookManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Book paginated",
        Description = "",
        OperationId = "BookManagement.GetAll",
        Tags = new[] { "BookManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<BookResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<BookResponse>>> HandleAsync(
        [FromQuery] GetAllBookPaginatedRequest query,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Book>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Title))
            queryable = queryable.Where(e => EF.Functions.Like(e.Title, $"%{query.Title}%"));

        if (!string.IsNullOrWhiteSpace(query.Code))
            queryable = queryable.Where(e => EF.Functions.Like(e.Code!, $"{query.Code}"));

        if (!string.IsNullOrWhiteSpace(query.Author))
            queryable = queryable.Where(e => EF.Functions.Like(e.Author!, $"{query.Author}"));

        if (!string.IsNullOrWhiteSpace(query.Genre))
            queryable = queryable.Where(e => EF.Functions.Like(e.Genre!, $"{query.Genre}"));

        if (!string.IsNullOrWhiteSpace(query.Publisher))
            queryable = queryable.Where(e => EF.Functions.Like(e.Publisher!, $"{query.Publisher}"));

        if (!string.IsNullOrWhiteSpace(query.YearPublish))
            queryable = queryable.Where(e => e.YearPublish.ToString() == query.YearPublish);

        if (string.IsNullOrWhiteSpace(query.OrderBy))
            query.OrderBy = nameof(Domain.Entities.Book.CreatedAt);

        if (string.IsNullOrWhiteSpace(query.OrderType))
            query.OrderType = "DESC";

        queryable = queryable.OrderBy($"{query.OrderBy} {query.OrderType}");

        var Books = await queryable.Select(Book => new BookResponse
        {
            BookId = Book.BookId,
            Title = Book.Title,
            Code = Book.Title,
            Author = Book.Author,
            Publisher = Book.Publisher,
            YearPublish = Book.YearPublish,
            Description = Book.Description,
            Genre = Book.Genre,
            Price = Book.Price,
            Qty = Book.QtyAvailable,

            CreatedAt = Book.CreatedAt,
            CreatedByName = Book.CreatedByName,
            LastUpdatedAt = Book.LastUpdatedAt,
            LastUpdatedByName = Book.LastUpdatedByName
        })
        .Skip(query.CalculateSkip())
        .Take(query.Size)
        .ToListAsync(cancellationToken);

        var response = new PagedList<BookResponse>(Books, query.Page, query.Size);

        return response;
    }
}