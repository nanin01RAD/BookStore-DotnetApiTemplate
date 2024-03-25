namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class CreateCartOrderRequest
{
    public Guid UserId { get; set; }
    public DateOnly? OrderDate { get; set; }
}

