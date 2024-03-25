using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Queries;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Responses;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement;

public class GetAllInventoryPaginated : BaseEndpoint<GetAllInventoryPaginatedRequest, PagedList<InventoryPaginatedResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllInventoryPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("Inventorys")]
    [Authorize]
    [RequiredScope(typeof(InventoryManagementScope), typeof(InventoryManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Inventory paginated",
        Description = "",
        OperationId = "InventoryManagement.GetAll",
        Tags = new[] { "InventoryManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<InventoryPaginatedResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<InventoryPaginatedResponse>>> HandleAsync(
        [FromQuery] GetAllInventoryPaginatedRequest query,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Inventory>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.BookTitle))
            queryable = queryable.Where(e => EF.Functions.Like(e.Book!.Title, $"%{query.BookTitle}%"));

        if (query.Qty > 0)
            queryable = queryable.Where(e => e.QtyCurrent == query.Qty);

        if (string.IsNullOrWhiteSpace(query.OrderBy))
            query.OrderBy = nameof(Domain.Entities.Inventory.CreatedAt);

        if (string.IsNullOrWhiteSpace(query.OrderType))
            query.OrderType = "DESC";

        queryable = queryable.OrderBy($"{query.OrderBy} {query.OrderType}");

        var Inventorys = await queryable.Select(Inventory => new InventoryPaginatedResponse
        {
            BookId = Inventory.BookId,
            BookTitle = Inventory.Book!.Title,
            Qty = Inventory.QtyCurrent,
            Status = Inventory.Status
        })
        .GroupBy(inventory => new { inventory.Status, inventory.BookId, inventory.BookTitle })
        .Select(group => new InventoryPaginatedResponse
        {
            BookId = group.Key.BookId,
            BookTitle = group.Key.BookTitle,
            Status = group.Key.Status,
            Qty = group.Sum(item => item.Qty)
        })
        .Skip(query.CalculateSkip())
        .Take(query.Size)
        .ToListAsync(cancellationToken);

        var response = new PagedList<InventoryPaginatedResponse>(Inventorys, query.Page, query.Size);

        return response;
    }
}