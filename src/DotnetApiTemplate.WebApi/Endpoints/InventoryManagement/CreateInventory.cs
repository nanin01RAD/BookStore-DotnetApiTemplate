using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.Shared.Abstractions.Encryption;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mail;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement;

public class CreateInventory : BaseEndpointWithoutResponse<CreateInventoryRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IInventoryService _inventoryService;
    private readonly IBookService _bookService;

    public CreateInventory(IDbContext dbContext,
        IInventoryService inventoryService,
        IBookService bookService)
    {
        _dbContext = dbContext;
        _inventoryService = inventoryService;
        _bookService = bookService;
    }

    [HttpPost("Inventory")]
    [Authorize]
    [RequiredScope(typeof(InventoryManagementScope))]
    [SwaggerOperation(
        Summary = "Create Inventory API",
        Description = "",
        OperationId = "InventoryManagement.CreateInventory",
        Tags = new[] { "InventoryManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateInventoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var Book = await _bookService.GetByIdAsync(request.BookId, cancellationToken);
        if(Book is null)
            return BadRequest(Error.Create("Book data not found"));

        var Inventory = new Inventory
        {
            BookId = request.BookId,
            QtyCurrent = request.Qty,
            Status = Domain.Enums.InventoryStatus.In,
        };

        await _inventoryService.CreateAsync(Inventory, cancellationToken);

        //===========Update qty tbl Book
        _dbContext.AttachEntity(Book);

        if (request.Qty != Book.QtyAvailable)
            Book.QtyAvailable = request.Qty;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}