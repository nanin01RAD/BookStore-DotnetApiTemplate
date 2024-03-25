using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Queries;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Responses;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;
using DotnetApiTemplate.Core.Abstractions;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement;

public class GetAllCart : BaseEndpoint<GetAllCartRequest, PagedList<CartResponse>>
{
    private readonly IDbContext _dbContext;
    private readonly ICartService _cartService;

    public GetAllCart(IDbContext dbContext,
        ICartService cartService)
    {
        _dbContext = dbContext;
        _cartService = cartService;

    }

    [HttpGet("Carts")]
    [Authorize]
    [RequiredScope(typeof(CartScope), typeof(CartScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Cart list",
        Description = "",
        OperationId = "Cart.GetAll",
        Tags = new[] { "Cart" })
    ]
    [ProducesResponseType(typeof(PagedList<CartResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<CartResponse>>> HandleAsync(
        [FromQuery] GetAllCartRequest query,
        CancellationToken cancellationToken = new())
    {
        var queryable = _cartService.GetBaseQuery()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.UserName))
            queryable = queryable.Where(e => EF.Functions.Like(e.User!.FullName, $"%{query.UserName}%"));

        if (!string.IsNullOrWhiteSpace(query.BookTitle))
            queryable = queryable.Where(e => EF.Functions.Like(e.Book!.Title!, $"{query.BookTitle}"));

        if (string.IsNullOrWhiteSpace(query.OrderBy))
            query.OrderBy = nameof(Domain.Entities.Cart.CreatedAt);

        if (string.IsNullOrWhiteSpace(query.OrderType))
            query.OrderType = "DESC";

        queryable = queryable.OrderBy($"{query.OrderBy} {query.OrderType}");

        var Carts = await queryable.Select(Cart => new CartResponse
        {
            CartId = Cart.CartId,
            BookId = Cart.BookId,
            UserId = Cart.BookId,
            UserName = Cart.User!.FullName,
            BookTitle = Cart.Book!.Title,
            Price = Cart.Price,
            Qty = Cart.Qty,
            TotalPrice = Cart.TotalPrice,

            CreatedAt = Cart.CreatedAt,
            CreatedByName = Cart.CreatedByName,
            LastUpdatedAt = Cart.LastUpdatedAt,
            LastUpdatedByName = Cart.LastUpdatedByName
        })
        .Skip(query.CalculateSkip())
        .Take(query.Size)
        .ToListAsync(cancellationToken);

        var response = new PagedList<CartResponse>(Carts, query.Page, query.Size);

        return response;
    }
}