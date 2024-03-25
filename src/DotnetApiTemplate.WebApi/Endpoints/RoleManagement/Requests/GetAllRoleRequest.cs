using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Requests;

public class GetAllRoleRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}