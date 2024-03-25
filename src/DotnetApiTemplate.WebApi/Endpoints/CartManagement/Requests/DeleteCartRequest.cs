using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;

public class DeleteCartRequest
{
    [FromRoute(Name = "CartId")] public Guid CartId { get; set; }
}