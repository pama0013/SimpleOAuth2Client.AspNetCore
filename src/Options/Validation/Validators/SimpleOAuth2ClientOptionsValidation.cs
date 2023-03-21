using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

/// <summary>
/// Customized options validator for SimpleOAuth2ClientOptions.
/// </summary>
/// <remarks>This validator use a FluentValidation validator.</remarks>
internal sealed class SimpleOAuth2ClientOptionsValidation : IValidateOptions<SimpleOAuth2ClientOptions>
{
    private readonly IValidator<SimpleOAuth2ClientOptions> _validator;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="validator">The fluent validation validator.</param>
    /// <exception cref="ArgumentNullException">If the constructor parameter is null.</exception>
    public SimpleOAuth2ClientOptionsValidation(IValidator<SimpleOAuth2ClientOptions> validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, SimpleOAuth2ClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        ValidationResult results = _validator.Validate(options);
        if (!results.IsValid)
        {
            return ValidateOptionsResult.Fail(results.ToString());
        }

        return ValidateOptionsResult.Success;
    }
}
