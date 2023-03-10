using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

/// <summary>
/// Customized options validator for ClientCredentialOptions.
/// </summary>
/// <remarks>This validator use a FluentValidation validator.</remarks>
internal sealed class ClientCredentialOptionsValidation : IValidateOptions<ClientCredentialOptions>
{
    private readonly IValidator<ClientCredentialOptions> _validator;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="validator">The fluent validation validator.</param>
    /// <exception cref="ArgumentNullException">If the constructor parameter is null.</exception>
    public ClientCredentialOptionsValidation(IValidator<ClientCredentialOptions> validator)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string? name, ClientCredentialOptions options)
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
