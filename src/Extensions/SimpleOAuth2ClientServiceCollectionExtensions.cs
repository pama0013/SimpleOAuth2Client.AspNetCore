using System.Net.Http.Headers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Extensions;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;

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
    public static IServiceCollection AddSimpleOAuth2Client(this IServiceCollection services, Action<ClientCredentialOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services
            .AddSingleton<IValidator<ClientCredentialOptions>, ClientCredentialOptionsValidator>()
            .AddOptions<ClientCredentialOptions>()
            .Configure(configureOptions)
            .AddClientCredentialOptionsValidation()
            .ValidateOnStart();

        RegisterSimpleOAuth2Client(services);

        return services;
    }

    private static void RegisterSimpleOAuth2Client(IServiceCollection services)
    {
        services
            .AddHttpClient("AuthClient", httpClient =>
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            });

        services
            .AddSingleton<IAuthorizationServerErrorHandler, ClientCredentialsErrorHandler>()
            .AddSingleton<IAuthorizationGrant, ClientCredentials>()
            .AddSingleton<IOAuth2Client, OAuth2Client>();
    }
}
