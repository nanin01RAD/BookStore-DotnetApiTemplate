using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;

public class GetCartByIdRequest
{
    [FromRoute(Name = "CartId")] public Guid CartId { get; set; }
}