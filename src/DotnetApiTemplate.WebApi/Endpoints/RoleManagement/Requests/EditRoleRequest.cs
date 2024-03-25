using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Requests;

public class EditRoleRequest
{
    [FromRoute] public Guid RoleId { get; set; }
    [FromBody] public EditRoleRequestPayload Payload { get; set; } = null!;
}