using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartOrder.Scopes;
using DotnetApiTemplate.WebApi.Mapping;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder;

public class CreateCartOrder : BaseEndpointWithoutResponse<CreateCartOrderRequest>
{
    private readonly IDbContext _dbContext;
    private readonly ICartService _cartService;
    private readonly ICartOrderService _cartOrderService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;

    public CreateCartOrder(IDbContext dbContext,
        ICartService cartService,
        ICartOrderService cartOrderService,
        IBookService bookService,
        IUserService userService)
    {
        _dbContext = dbContext;
        _cartService = cartService;
        _cartOrderService = cartOrderService;
        _bookService = bookService;
        _userService = userService;
    }

    [HttpPost("CartOrder")]
    [Authorize]
    [RequiredScope(typeof(CartOrderScope))]
    [SwaggerOperation(
        Summary = "Create CartOrder API",
        Description = "",
        OperationId = "CartOrder.CreateCartOrder",
        Tags = new[] { "CartOrder" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateCartOrderRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateCartOrderRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("invalid-parameter", validationResult.Construct()));

        var User = await _userService.GetByIdAsync(request.UserId, cancellationToken);
        if (User is null)
            return BadRequest("User not found");

        var CartOrder = new Domain.Entities.Order
        {
            UserId = request.UserId,
            OrderDate = request.OrderDate,
            Status = Domain.Enums.OrderStatus.Onprocess
        };

        var CartItems = await _cartService.GetBaseQuery()
            .Where(e => e.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        if (User is null)
            return BadRequest("User not found");

        if (CartItems is not null)
        {
            foreach (var i in CartItems)
            {
                var Book = await _bookService.GetByIdAsync(i.BookId, cancellationToken);
                if (Book is null)
                    return BadRequest("Book not found");

                CartOrder.OrderDetails.Add(new Domain.Entities.OrderDetail
                {
                    OrderId = CartOrder.OrderId,
                    BookId = i.BookId,
                    Price = i.Price,
                    Qty = i.Qty,
                    TotalPrice = i.Price * i.Qty
                });
            }

            CartOrder.Total = CartOrder.OrderDetails.Sum(e => e.TotalPrice);
        }

        await _cartOrderService.CreateAsync(CartOrder, cancellationToken);

        //====================Delete data pada tabel cart
        if (CartItems is not null)
        {
            foreach (var i in CartItems)
            {
                await _cartService.DeleteAsync(i.CartId, cancellationToken);
            }
        }

        return NoContent();
    }
}