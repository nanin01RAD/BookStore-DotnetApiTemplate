using DotnetApiTemplate.Domain.Enums;
using DotnetApiTemplate.WebApi.Contracts.Responses;

namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Responses;

public class InventoryResponse : BaseResponse
{
    public Guid? InventoryId { get; set; }
    public Guid? BookId { get; set; }
    public string? BookTitle { get; set; }
    public int? Qty { get; set; }
    public InventoryStatus? Status { get; set; }
}