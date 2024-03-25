using DotnetApiTemplate.Shared.Abstractions.Queries;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;

public class GetAllCartRequest : BasePaginationCalculation
{
    public string? UserName { get; set; }
    public string? BookTitle { get; set; }
}