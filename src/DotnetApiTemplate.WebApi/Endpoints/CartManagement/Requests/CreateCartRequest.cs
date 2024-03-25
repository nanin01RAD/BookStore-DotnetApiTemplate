namespace DotnetApiTemplate.WebApi.Endpoints.CartManagement.Requests;

public class CreateCartRequest
{
    public Guid? UserId { get; set; }
    public Guid? BookId { get; set; }
    public int? Qty { get; set; }
}