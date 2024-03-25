using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Responses;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Scopes;
using DotnetApiTemplate.WebApi.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement;

public class GetCartById : BaseEndpoint<GetCartByIdRequest, CartResponse>
{
    private readonly ICartService _cartService;

    public GetCartById(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("Cart/{CartId}")]
    [Authorize]
    [RequiredScope(typeof(CartScope), typeof(CartScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Cart by id",
        Description = "",
        OperationId = "Cart.GetById",
        Tags = new[] { "Cart" })
    ]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<CartResponse>> HandleAsync([FromRoute] GetCartByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var Cart = await _cartService.GetByIdAsync(request.CartId, cancellationToken);
        if (Cart is null)
            return BadRequest(Error.Create("Data cart not found"));

        return new CartResponse
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
        };
    }
}