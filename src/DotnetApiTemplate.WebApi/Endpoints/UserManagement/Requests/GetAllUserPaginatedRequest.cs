using DotnetApiTemplate.Shared.Abstractions.Queries;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;

public class GetAllUserPaginatedRequest : BasePaginationCalculation
{
    public string? Username { get; set; }
    public string? Fullname { get; set; }
}