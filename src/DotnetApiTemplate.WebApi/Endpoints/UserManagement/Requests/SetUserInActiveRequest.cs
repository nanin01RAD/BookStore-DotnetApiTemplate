using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;

public class SetUserInActiveRequest
{
    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
}