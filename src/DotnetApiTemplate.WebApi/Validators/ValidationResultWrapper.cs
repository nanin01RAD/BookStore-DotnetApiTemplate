using DotnetApiTemplate.Shared.Abstractions.Helpers;
using FluentValidation.Results;

namespace DotnetApiTemplate.WebApi.Validators;

public static class ValidationResultWrapper
{
    public static List<ValidationError>? Construct(this ValidationResult result)
    {
        return !result.Errors.Any()
            ? null
            : result.Errors.Select(item =>
                    new ValidationError(
                        item.PropertyName.ToCamelCase(),
                        item.AttemptedValue,
                        item.ErrorCode,
                        item.ErrorMessage))
                .ToList();
    }
}