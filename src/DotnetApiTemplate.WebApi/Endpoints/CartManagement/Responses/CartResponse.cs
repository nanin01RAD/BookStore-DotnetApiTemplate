using DotnetApiTemplate.WebApi.Contracts.Responses;

namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Responses;

public class CartResponse : BaseResponse
{
    public Guid? CartId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public Guid? BookId { get; set; }
    public string? BookTitle { get; set; }
    public decimal? Price { get; set; }
    public int? Qty { get; set; }
    public decimal? TotalPrice { get; set; }
}