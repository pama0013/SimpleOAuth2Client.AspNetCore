using FluentValidation;

namespace SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

/// <summary>
/// Customized options validator for ClientCredentialOptions.
/// </summary>
/// <remarks>This validator use a FluentValidation validator.</remarks>
internal sealed class ClientCredentialOptionsValidator : AbstractValidator<ClientCredentialOptions>
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <remarks>The constructor include all validation rules.</remarks>
    public ClientCredentialOptionsValidator()
    {
        RuleFor(options => options.ClientId)
            .NotNull()
            .WithMessage("The ClientId is null but required for the OAuth2 protocol. Please add a value for ClientId.");

        RuleFor(options => options.ClientSecret)
            .NotNull()
            .WithMessage("The ClientSecret is null but required for the OAuth2 protocol. Please add a value for ClientSecret.");

        RuleFor(options => options.TokenEndpoint)
            .NotNull()
            .WithMessage("The TokenEndpoint is null but required for the OAuth2 protocol. Please add a value for TokenEndpoint.");
    }
}
