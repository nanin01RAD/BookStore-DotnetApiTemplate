namespace DotnetApiTemplate.WebApi.Endpoints.InventoryManagement.Requests;

public class CreateInventoryRequest
{
    public Guid BookId { get; set; }
    public int? Qty { get; set; }
}