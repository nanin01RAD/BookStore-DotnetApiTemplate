using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class UpdateCartOrderRequestPayloadValidator : AbstractValidator<UpdateCartOrderRequestPayload>
{
    public UpdateCartOrderRequestPayloadValidator()
    {
        RuleFor(e => e.OrderDate).NotNull().NotEmpty();
    }
}