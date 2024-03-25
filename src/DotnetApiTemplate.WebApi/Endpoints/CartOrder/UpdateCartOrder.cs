using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder;

public class UpdateCartOrder : BaseEndpointWithoutResponse<UpdateCartOrderRequest>
{
    private readonly ICartOrderService _cartOrderService;
    private readonly IBookService _bookService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<UpdateCartOrder> _localizer;

    public UpdateCartOrder(ICartOrderService cartOrderService,
        IBookService bookService,
        IDbContext dbContext,
        IStringLocalizer<UpdateCartOrder> localizer)
    {
        _bookService = bookService;
        _cartOrderService = cartOrderService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPut("CartOrder/{OrderId}")]
    [Authorize]
    [RequiredScope(typeof(CartOrderScope))]
    [SwaggerOperation(
        Summary = "Update CartOrder API",
        Description = "",
        OperationId = "CartOrder.UpdateCartOrder",
        Tags = new[] { "CartOrder" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateCartOrderRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new UpdateCartOrderRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.UpdateCartOrderRequestPayload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var CartOrder = await _cartOrderService.GetByExpressionAsync(
            e => e.OrderId == request.OrderId,
            e => new Order
            {
                OrderId = e.OrderId,
                UserId = e.UserId,
                OrderDate = e.OrderDate,
                Status = e.Status,
                Total = e.Total
            }, cancellationToken);

        if (CartOrder is null)
            return BadRequest(Error.Create("Order data not found"));

        if (CartOrder.Status != Domain.Enums.OrderStatus.Onprocess)
            return BadRequest(Error.Create("The Order has been processed"));

        _dbContext.AttachEntity(CartOrder);

        if (request.UpdateCartOrderRequestPayload.Items is not null)
        {
            foreach (var i in request.UpdateCartOrderRequestPayload.Items)
            {
                var cartOrderDetail = await _dbContext.Set<Domain.Entities.OrderDetail>()
                    .Where(e => e.OrderDetailId == i.OrderDetailId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (cartOrderDetail is null)
                    return BadRequest("Detail not found");

                var Book = await _bookService.GetByIdAsync(i.BookId!.Value, cancellationToken);
                if (Book is null)
                    return BadRequest("Book not found");

                if (i.BookId != cartOrderDetail.BookId)
                    cartOrderDetail.BookId = i.BookId!.Value;

                if (i.Price != cartOrderDetail.Price)
                    cartOrderDetail.Price = i.Price;

                if (i.Qty != cartOrderDetail.Qty)
                    cartOrderDetail.Qty = i.Qty;

                cartOrderDetail.TotalPrice = i.Price * i.Qty;

                _dbContext.Set<Domain.Entities.OrderDetail>().Update(cartOrderDetail);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            
            CartOrder.Total = CartOrder.OrderDetails.Sum(e => e.TotalPrice);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}