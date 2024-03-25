using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Responses;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement;

public class GetInventoryByBookId : BaseEndpoint<GetInventoryByIdRequest, List<InventoryResponse>>
{
    private readonly IInventoryService _InventoryService;
    private readonly IStringLocalizer<GetInventoryByBookId> _localizer;

    public GetInventoryByBookId(IInventoryService InventoryService,
        IStringLocalizer<GetInventoryByBookId> localizer)
    {
        _InventoryService = InventoryService;
        _localizer = localizer;
    }

    [HttpGet("Inventorys/{InventoryId}")]
    [Authorize]
    [RequiredScope(typeof(InventoryManagementScope), typeof(InventoryManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Inventory by id",
        Description = "",
        OperationId = "InventoryManagement.GetById",
        Tags = new[] { "InventoryManagement" })
    ]
    [ProducesResponseType(typeof(List<InventoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<List<InventoryResponse>>> HandleAsync([FromRoute] GetInventoryByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var Inventory = await _InventoryService.GetByBookIdAsync(request.BookId, cancellationToken);
        if (Inventory is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        return Inventory.ConvertAll(e => new InventoryResponse
        {
            InventoryId = e.InventoryId,
            BookId = e.BookId,
            BookTitle = e.Book!.Title,
            Qty = e.QtyCurrent,
            Status = e.Status,

            CreatedAt = e.CreatedAt,
            CreatedByName = e.CreatedByName,
            LastUpdatedAt = e.LastUpdatedAt,
            LastUpdatedByName = e.LastUpdatedByName
        }).ToList();
    }
}