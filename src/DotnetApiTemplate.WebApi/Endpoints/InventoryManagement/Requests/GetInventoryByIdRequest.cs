using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;

public class GetInventoryByIdRequest
{
    [FromRoute(Name = "BookId")] public Guid BookId { get; set; }
}