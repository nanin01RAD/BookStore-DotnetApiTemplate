using DotnetApiTemplate.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DotnetApiTemplate.WebApi.Endpoints.RoleManagement.Requests;

public class GetAllRolePaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}