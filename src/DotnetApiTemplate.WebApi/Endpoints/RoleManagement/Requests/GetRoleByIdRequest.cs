using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Requests;

public class GetRoleByIdRequest
{
    [FromRoute] public Guid RoleId { get; set; }
}