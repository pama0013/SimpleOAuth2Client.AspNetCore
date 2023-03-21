using FluentValidation;

namespace SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

/// <summary>
/// Customized options validator for RetryOptions.
/// </summary>
/// <remarks>This validator use a FluentValidation validator.</remarks>
internal sealed class RetryOptionsValidator : AbstractValidator<RetryOptions>
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <remarks>The constructor include all validation rules.</remarks>
    public RetryOptionsValidator()
    {
        RuleFor(retryOptions => retryOptions.RetryAttempts)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The RetryAttempts can not be negative. Please add a value that is greater or equal then zero.");

        RuleFor(retryOptions => retryOptions.TimeoutPerRetry)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The TimeoutPerRetry can not be negative. Please add a value that is greater or equal then zero.");

        RuleFor(retryOptions => retryOptions.FirstRetryDelay)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The FirstRetryDelay can not be negative. Please add a value that is greater or equal then zero.");
    }
}
