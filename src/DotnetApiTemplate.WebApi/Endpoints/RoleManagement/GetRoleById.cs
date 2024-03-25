using DotnetApiTemplate.Domain.Entities;
using DotnetApiTemplate.Shared.Abstractions.Databases;
using DotnetApiTemplate.WebApi.Common;
using DotnetApiTemplate.WebApi.Contracts.Responses;
using DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Requests;
using DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Scopes;
using DotnetApiTemplate.WebApi.Mapping;
using DotnetApiTemplate.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace DotnetApiTemplate.WebApi.Endpoints.RoleManagement;

public class GetRoleById : BaseEndpoint<GetRoleByIdRequest, RoleResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<GetRoleById> _localizer;

    public GetRoleById(IDbContext dbContext,
        IStringLocalizer<GetRoleById> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpGet("roles/{RoleId}")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles by ID",
        Description = "",
        OperationId = "RoleManagement.GetRoleById",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<RoleResponse>> HandleAsync([FromRoute] GetRoleByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new GetRoleByIdRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var role = await _dbContext.Set<Role>()
            .Include(e => e.RoleScopes)
            .Where(e => e.RoleId == request.RoleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        return role.ToRoleResponse();
    }
}