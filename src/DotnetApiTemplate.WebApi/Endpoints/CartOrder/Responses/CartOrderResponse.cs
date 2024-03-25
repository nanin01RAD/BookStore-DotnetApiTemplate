using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.WebApi.Contracts.Responses;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Responses;

public class CartOrderResponse : BaseResponse
{
    public Guid? OrderId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public DateOnly? OrderDate { get; set; }
    public int? TotalQty { get; set; }
    public decimal? TotalPrice { get; set; }
    public OrderStatus? Status { get; set; }
}