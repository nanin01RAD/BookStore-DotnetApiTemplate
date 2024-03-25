using DotnetApiTemplate.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class UpdateStatusCartOrderRequest
{
    [FromRoute(Name = "CartOrderId")] public Guid OrderId { get; set; }
    [FromRoute(Name = "Status")] public OrderStatus Status { get; set; }
}