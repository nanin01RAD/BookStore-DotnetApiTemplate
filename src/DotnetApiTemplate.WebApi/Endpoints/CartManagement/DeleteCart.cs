using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.CartManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement;

public class DeleteCart : BaseEndpointWithoutResponse<DeleteCartRequest>
{
    private readonly ICartService _cartService;
    private readonly IDbContext _dbContext;

    public DeleteCart(ICartService cartService,
        IDbContext dbContext)
    {
        _cartService = cartService;
        _dbContext = dbContext;
    }

    [HttpDelete("Cart/{CartId}")]
    [Authorize]
    [RequiredScope(typeof(CartScope))]
    [SwaggerOperation(
        Summary = "Delete Cart API",
        Description = "",
        OperationId = "Cart.DeleteCart",
        Tags = new[] { "Cart" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteCartRequest request,
        CancellationToken cancellationToken = new())
    {
        var Cart = await _cartService.GetByIdAsync(request.CartId, cancellationToken);
        if (Cart is null)
            return BadRequest(Error.Create("Order data not found"));

        await _cartService.DeleteAsync(request.CartId, cancellationToken);

        return NoContent();
    }
}