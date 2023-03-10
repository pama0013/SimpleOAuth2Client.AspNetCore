using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

namespace SimpleOAuth2Client.AspNetCore.Options.Validation.Extensions;

/// <summary>
/// Extension methods to add customized options validation to the ASP.NET Core Service OptionsBuilder.
/// </summary>
internal static class OptionsBuilderFluentValidationExtensions
{
    /// <summary>
    /// Add all ClientCredentialOptions related services.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    /// <returns>The modified options builder.</returns>
    internal static OptionsBuilder<ClientCredentialOptions> AddClientCredentialOptionsValidation(
        this OptionsBuilder<ClientCredentialOptions> optionsBuilder)
    {
        optionsBuilder
            .Services
            .AddSingleton<IValidator<ClientCredentialOptions>, ClientCredentialOptionsValidator>()
            .AddSingleton<IValidateOptions<ClientCredentialOptions>, ClientCredentialOptionsValidation>();

        return optionsBuilder;
    }
}
