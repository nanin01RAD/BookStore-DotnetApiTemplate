using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Domain.Enums;
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

public class UpdateStatusCartOrder : BaseEndpointWithoutResponse<UpdateStatusCartOrderRequest>
{
    private readonly ICartOrderService _cartOrderService;
    private readonly IInventoryService _inventoryService;
    private readonly IBookService _bookService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<UpdateStatusCartOrder> _localizer;

    public UpdateStatusCartOrder(ICartOrderService cartOrderService,
        IInventoryService inventoryService,
        IBookService bookService,
        IDbContext dbContext,
        IStringLocalizer<UpdateStatusCartOrder> localizer)
    {
        _bookService = bookService;
        _cartOrderService = cartOrderService;
        _inventoryService = inventoryService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPut("CartOrder/Status/{OrderId}")]
    [Authorize]
    [RequiredScope(typeof(CartOrderScope))]
    [SwaggerOperation(
        Summary = "Update Status CartOrder API",
        Description = "",
        OperationId = "CartOrder.PaymentCartOrder",
        Tags = new[] { "CartOrder" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateStatusCartOrderRequest request,
        CancellationToken cancellationToken = new())
    {
        var CartOrder = await _cartOrderService.GetByIdAsync(request.OrderId, cancellationToken);

        if (CartOrder is null)
            return BadRequest(Error.Create("Order data not found"));

        if (CartOrder.Status != Domain.Enums.OrderStatus.Payment)
            return BadRequest(Error.Create("The Order has been payment, barang yang sudah dibeli tidak bisa dikembalikan"));

        _dbContext.AttachEntity(CartOrder);
        
        CartOrder.Status = request.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);

        //=====Insert inventory
        if (CartOrder.Status == Domain.Enums.OrderStatus.Payment)
        {
            var CartOrderDetail = await _cartOrderService.GetDetailByIdAsync(request.OrderId);

            if (CartOrderDetail is null)
                return BadRequest(Error.Create("Order detail data not found"));

            foreach (var item in CartOrderDetail)
            {
                var Book = await _bookService.GetByIdAsync(item.BookId, cancellationToken);
                if (Book is null)
                    return BadRequest(Error.Create("Book data not found"));

                var Inventory = new Inventory
                {
                    BookId = item.BookId,
                    QtyCurrent = item.Qty,
                    Status = Domain.Enums.InventoryStatus.Out,
                };

                await _inventoryService.CreateAsync(Inventory, cancellationToken);
            }
        }

        return NoContent();
    }
}