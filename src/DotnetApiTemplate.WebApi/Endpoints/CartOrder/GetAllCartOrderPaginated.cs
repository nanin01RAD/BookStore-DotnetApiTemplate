using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Queries;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Responses;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder;

public class GetAllCartOrderPaginated : BaseEndpoint<GetAllCartOrderPaginatedRequest, PagedList<CartOrderResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllCartOrderPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("CartOrders")]
    [Authorize]
    [RequiredScope(typeof(CartOrderScope), typeof(CartOrderScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get CartOrder paginated",
        Description = "",
        OperationId = "CartOrder.GetAll",
        Tags = new[] { "CartOrder" })
    ]
    [ProducesResponseType(typeof(PagedList<CartOrderResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<CartOrderResponse>>> HandleAsync(
        [FromQuery] GetAllCartOrderPaginatedRequest query,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Order>()
            .Include(e => e.User)
            .Include(e => e.OrderDetails)
            .ThenInclude(f => f.Book)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.UserName))
            queryable = queryable.Where(e => EF.Functions.Like(e.User!.FullName, $"%{query.UserName}%"));

        if (query.OrderDate.HasValue)
            queryable = queryable.Where(e => e.OrderDate == query.OrderDate);

        if (query.Status.HasValue)
            queryable = queryable.Where(e => e.Status == query.Status);

        if (string.IsNullOrWhiteSpace(query.OrderBy))
            query.OrderBy = nameof(CartOrderResponse.CreatedAt);

        if (string.IsNullOrWhiteSpace(query.OrderType))
            query.OrderType = "DESC";

        queryable = queryable.OrderBy($"{query.OrderBy} {query.OrderType}");

        var CartOrder = await queryable.Select(e => new CartOrderResponse
        {
            OrderId = e.OrderId,
            UserId = e.UserId,
            UserName = e.User!.FullName,
            OrderDate = e.OrderDate,
            TotalQty = e.OrderDetails.Count(),
            TotalPrice = e.Total,
            Status = e.Status,
            CreatedAt = e.CreatedAt,
            CreatedByName = e.CreatedByName,
            LastUpdatedAt = e.LastUpdatedAt,
            LastUpdatedByName = e.LastUpdatedByName
        })
        .Skip(query.CalculateSkip())
        .Take(query.Size)
        .ToListAsync(cancellationToken);

        var response = new PagedList<CartOrderResponse>(CartOrder, query.Page, query.Size);

        return response;
    }
}