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
    /// Add all SimpleOAuth2ClientOptions related services.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    /// <returns>The modified options builder.</returns>
    internal static OptionsBuilder<SimpleOAuth2ClientOptions> AddSimpleOAuth2ClientOptionsValidation(
        this OptionsBuilder<SimpleOAuth2ClientOptions> optionsBuilder)
    {
        optionsBuilder
            .Services
            .AddSingleton<IValidator<SimpleOAuth2ClientOptions>, SimpleOAuth2ClientOptionsValidator>()
            .AddSingleton<IValidateOptions<SimpleOAuth2ClientOptions>, SimpleOAuth2ClientOptionsValidation>();

        return optionsBuilder;
    }
}
