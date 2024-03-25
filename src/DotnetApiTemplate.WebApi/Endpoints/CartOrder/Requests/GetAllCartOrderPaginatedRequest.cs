using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.Shared.Abstractions.Queries;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class GetAllCartOrderPaginatedRequest : BasePaginationCalculation
{
    public string? UserName { get; set; }
    public DateOnly? OrderDate { get; set; }
    public OrderStatus ? Status { get; set; }
}