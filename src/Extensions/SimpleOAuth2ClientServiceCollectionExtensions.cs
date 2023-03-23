using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SimpleOAuth2Client.AspNetCore.Common.Http.Extensions;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.Model.Validators;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Extensions;

namespace SimpleOAuth2Client.AspNetCore.Extensions;

/// <summary>
/// Extension methods to add SimpleOAuth2Client to the ASP.NET Core Service Collection.
/// </summary>
public static class SimpleOAuth2ClientServiceCollectionExtensions
{
    /// <summary>
    /// Add all services to ASP.NET Core service collection.
    /// </summary>
    /// <param name="services">The service collection to which OAuth2 services are added.</param>
    /// <param name="configureOptions">A delegate which is run to configure the services</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddSimpleOAuth2Client(this IServiceCollection services, Action<SimpleOAuth2ClientOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services
            .AddOptions<SimpleOAuth2ClientOptions>()
            .Configure(configureOptions)
            .AddSimpleOAuth2ClientOptionsValidation()
            .ValidateOnStart();

        services
            .AddCustomizedHttpClient()
            .AddSimpleOAuth2ClientServices();

        return services;
    }

    private static IServiceCollection AddSimpleOAuth2ClientServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IAuthorizationGrant, ClientCredentials>()
            .AddSingleton<IOAuth2Client, OAuth2Client>()
            .AddSingleton<IValidator<AccessTokenResponse>, AccessTokenResponseValidator>();

        return services;
    }
}
