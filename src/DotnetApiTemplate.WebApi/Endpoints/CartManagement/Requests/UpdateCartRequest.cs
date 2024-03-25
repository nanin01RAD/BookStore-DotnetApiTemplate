using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;

public class UpdateCartRequest
{
    public UpdateCartRequest()
    {
        UpdateCartRequestPayload = new UpdateCartRequestPayload();
    }

    [FromRoute(Name = "CartId")] public Guid CartId { get; set; }
    [FromBody] public UpdateCartRequestPayload UpdateCartRequestPayload { get; set; }
}

public class UpdateCartRequestPayload
{
    public Guid? UserId { get; set; }
    public Guid? BookId { get; set; }
    public int? Qty { get; set; }
}