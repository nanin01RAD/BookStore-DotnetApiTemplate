using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Infrastructure.Services;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement;

public class DeleteInventory : BaseEndpointWithoutResponse<DeleteInventoryRequest>
{
    private readonly IInventoryService _inventoryService;
    private readonly IBookService _bookService;
    private readonly IDbContext _dbContext;

    public DeleteInventory(IInventoryService inventoryService,
        IBookService bookService,
        IDbContext dbContext)
    {
        _inventoryService = inventoryService;
        _bookService = bookService;
        _dbContext = dbContext;
    }

    [HttpDelete("Inventory/{InventoryId}")]
    [Authorize]
    [RequiredScope(typeof(InventoryManagementScope))]
    [SwaggerOperation(
        Summary = "Delete Inventory API",
        Description = "",
        OperationId = "InventoryManagement.DeleteInventory",
        Tags = new[] { "InventoryManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteInventoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var Inventory = await _inventoryService.GetByIdAsync(request.InventoryId, cancellationToken);
        if (Inventory is null)
            return BadRequest(Error.Create("Order data not found"));

        //============before delete, update qty to tbl Book
        var Book = await _bookService.GetByIdAsync(Inventory.BookId, cancellationToken);
        if (Book is null)
            return BadRequest(Error.Create("Book data not found"));

        _dbContext.AttachEntity(Book);

        if (Inventory.QtyCurrent != Book.QtyAvailable)
            Book.QtyAvailable = (Inventory.Status == Domain.Enums.InventoryStatus.In ? 
                (Book.QtyAvailable - Inventory.QtyCurrent) :
                (Book.QtyAvailable + Inventory.QtyCurrent));

        await _dbContext.SaveChangesAsync(cancellationToken);

        //=======Delete data inventory
        await _inventoryService.DeleteAsync(request.InventoryId, cancellationToken);

        return NoContent();
    }
}