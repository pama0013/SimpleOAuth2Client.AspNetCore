using FluentValidation;
using SimpleOAuth2Client.AspNetCore.Configuration;

namespace SimpleOAuth2Client.AspNetCore.Validators;

/// <summary>
/// A costume validator for the ClientCredentialConfig.
/// </summary>
internal sealed class ClientCredentialConfigValidator : AbstractValidator<ClientCredentialConfig>
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <remarks>The constructor include all costume validation rules.</remarks>
    public ClientCredentialConfigValidator()
    {
        RuleFor(config => config.ClientId).NotNull();
        RuleFor(config => config.ClientSecret).NotNull();
        RuleFor(config => config.TokenEndpoint).NotNull();
    }
}
