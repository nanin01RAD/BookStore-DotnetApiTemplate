using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class GetCartOrderDetailByIdRequest
{
    [FromRoute(Name = "CartOrderDetailId")] public Guid OrderDetailId { get; set; }
}