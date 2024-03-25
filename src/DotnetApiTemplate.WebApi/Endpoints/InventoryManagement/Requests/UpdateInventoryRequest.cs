using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;

public class UpdateInventoryRequest
{
    public UpdateInventoryRequest()
    {
        UpdateInventoryRequestPayload = new UpdateInventoryRequestPayload();
    }

    [FromRoute(Name = "InventoryId")] public Guid InventoryId { get; set; }
    [FromBody] public UpdateInventoryRequestPayload UpdateInventoryRequestPayload { get; set; }
}

public class UpdateInventoryRequestPayload
{
    public Guid BookId { get; set; }
    public int? Qty { get; set; }
}