using DotnetApiTemplate.Shared.Abstractions.Queries;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;

public class GetAllInventoryPaginatedRequest : BasePaginationCalculation
{
    public string? BookTitle { get; set; }
    public int? Qty { get; set; }
}