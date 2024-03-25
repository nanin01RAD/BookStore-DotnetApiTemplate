using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.WebApi.Contracts.Responses;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Responses;

public class CartOrderDetailResponse : BaseResponse
{
    public Guid? OrderId { get; set; }
    public Guid? OrderDetailId { get; set; }
    public Guid? BookId { get; set; }
    public string? BookName { get; set; }
    public decimal? Price { get; set; }
    public int Qty { get; set; }
    public decimal Total { get; set; }
}