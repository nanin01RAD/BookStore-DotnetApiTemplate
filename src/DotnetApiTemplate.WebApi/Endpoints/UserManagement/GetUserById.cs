using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Contracts.Responses;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;
using DotnetApiTemplate.WebApi.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement;

public class GetUserById : BaseEndpoint<GetUserByIdRequest, UserResponse>
{
    private readonly IUserService _userService;
    private readonly IStringLocalizer<GetUserById> _localizer;

    public GetUserById(IUserService userService,
        IStringLocalizer<GetUserById> localizer)
    {
        _userService = userService;
        _localizer = localizer;
    }

    [HttpGet("users/{userId}")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user by id",
        Description = "",
        OperationId = "UserManagement.GetById",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UserResponse>> HandleAsync([FromRoute] GetUserByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        return user.ToUserResponse();
    }
}