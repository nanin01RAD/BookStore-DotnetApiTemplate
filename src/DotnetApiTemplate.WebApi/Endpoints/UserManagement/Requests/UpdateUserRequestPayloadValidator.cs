using FluentValidation;

namespace DotnetApiTemplate.WebApi.Endpoints.UserManagement.Requests;

public class UpdateUserRequestPayloadValidator : AbstractValidator<UpdateUserRequestPayload>
{
    public UpdateUserRequestPayloadValidator()
    {
        RuleFor(e => e.Fullname).NotNull().NotEmpty();
    }
}