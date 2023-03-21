using FluentValidation;

namespace SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

/// <summary>
/// A customized validator for the SimpleOAuth2ClientOptions.
/// </summary>
internal sealed class SimpleOAuth2ClientOptionsValidator : AbstractValidator<SimpleOAuth2ClientOptions>
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <remarks>The constructor include all validation rules.</remarks>
    public SimpleOAuth2ClientOptionsValidator()
    {
        RuleFor(simpleOAuth2ClientOptions => simpleOAuth2ClientOptions.ClientCredentialOptions)
            .NotNull()
            .WithMessage("The options for grant type client_credential are not configured");

        RuleFor(simpleOAuth2ClientOptions => simpleOAuth2ClientOptions.ClientCredentialOptions)
            .SetValidator(new ClientCredentialOptionsValidator());

        RuleFor(simpleOAuth2ClientOptions => simpleOAuth2ClientOptions.RetryOptions)
            .NotNull()
            .WithMessage("The options for the retry handling are not configured");

        RuleFor(simpleOAuth2ClientOptions => simpleOAuth2ClientOptions.RetryOptions)
            .SetValidator(new RetryOptionsValidator());
    }
}
