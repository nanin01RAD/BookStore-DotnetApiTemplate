using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class DeleteCartOrderRequest
{
    [FromRoute(Name = "CartOrderId")] public Guid OrderId { get; set; }
}