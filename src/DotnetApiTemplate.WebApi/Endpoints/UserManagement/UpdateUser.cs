using DotnetApiTemplate.Core.Abstractions;
using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.UserManagement.Scopes;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement;

public class UpdateUser : BaseEndpointWithoutResponse<UpdateUserRequest>
{
    private readonly IUserService _userService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<UpdateUser> _localizer;

    public UpdateUser(IUserService userService,
        IDbContext dbContext,
        IStringLocalizer<UpdateUser> localizer)
    {
        _userService = userService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPut("users/{userId}")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Update user API",
        Description = "",
        OperationId = "UserManagement.UpdateUser",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateUserRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new UpdateUserRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.UpdateUserRequestPayload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var user = await _userService.GetByExpressionAsync(
            e => e.UserId == request.UserId,
            e => new User
            {
                UserId = e.UserId,
                FullName = e.FullName
            },
            cancellationToken);
        if (user is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        _dbContext.AttachEntity(user);

        if (request.UpdateUserRequestPayload.Fullname != user.FullName)
            user.FullName = request.UpdateUserRequestPayload.Fullname;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}