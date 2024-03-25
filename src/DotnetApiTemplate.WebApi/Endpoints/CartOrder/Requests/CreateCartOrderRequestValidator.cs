using DotnetApiTemplate.WebApi.Validators;
using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.CartOrder.Requests;

public class CreateCartOrderRequestValidator : AbstractValidator<CreateCartOrderRequest>
{
    public CreateCartOrderRequestValidator()
    {
        RuleFor(e => e.OrderDate).NotNull().NotEmpty();
    }
}