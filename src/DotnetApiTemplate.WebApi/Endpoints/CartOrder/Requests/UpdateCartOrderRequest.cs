using DotnetApiTemplate.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class UpdateCartOrderRequest
{
    public UpdateCartOrderRequest()
    {
        UpdateCartOrderRequestPayload = new UpdateCartOrderRequestPayload();
    }

    [FromRoute(Name = "CartOrderId")] public Guid OrderId { get; set; }
    [FromBody] public UpdateCartOrderRequestPayload UpdateCartOrderRequestPayload { get; set; }
}

public class UpdateCartOrderRequestPayload
{
    public Guid UserId { get; set; }
    public DateOnly? OrderDate { get; set; }
    public List<OrderDetailPayload>? Items { get; set; }
}

public record OrderDetailPayload
{
    public Guid? OrderDetailId { get; set; }
    public Guid? BookId { get; set; }
    public decimal? Price { get; set; }
    public int Qty { get; set; }
}