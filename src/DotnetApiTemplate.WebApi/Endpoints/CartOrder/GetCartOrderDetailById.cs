using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Responses;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Scopes;
using DotnetApiTemplate.WebApi.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder;

public class GetCartOrderDetailById : BaseEndpoint<GetCartOrderDetailByIdRequest, List<CartOrderDetailResponse>>
{
    private readonly ICartOrderService _cartOrderService;

    public GetCartOrderDetailById(ICartOrderService cartOrderService)
    {
        _cartOrderService = cartOrderService;
    }

    [HttpGet("CartOrder/Detail/{OrderId}")]
    [Authorize]
    [RequiredScope(typeof(CartOrderScope), typeof(CartOrderScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get CartOrder Detail by id",
        Description = "",
        OperationId = "CartOrder.GetDetailById",
        Tags = new[] { "CartOrder" })
    ]
    [ProducesResponseType(typeof(List<CartOrderDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<List<CartOrderDetailResponse>>> HandleAsync(
        [FromRoute] GetCartOrderDetailByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var CartOrderDetail = await _cartOrderService.GetDetailByIdAsync(request.OrderDetailId, cancellationToken);
        if (CartOrderDetail is null)
            return BadRequest(Error.Create("Data detail not found"));

        var detail = CartOrderDetail.ConvertAll(e => new CartOrderDetailResponse
        {
            OrderId = e.OrderId,
            OrderDetailId = e.OrderDetailId,
            BookId = e.BookId,
            BookName = e.Book!.Title,
            Price = e.Price,
            Qty = e.Qty!.Value,
            Total = e.Price!.Value * e.Qty!.Value
        });

        return detail;
    }
}