using System.Net.Http.Headers;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using SimpleOAuth2Client.AspNetCore.Configuration;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Validators;

namespace SimpleOAuth2Client.AspNetCore.Extensions;

/// <summary>
/// Extensions for ASP.NET Core Service Collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add all services to ASP.NET Core service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">The OAuth2 related configuration.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddSimpleOAuth2Client(this IServiceCollection services, Action<ClientCredentialConfig> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        ClientCredentialConfig configuration = new();
        configure(configuration);
        ValidateClientCredentialWorkflowConfig(configuration);
        RegisterSimpleOAuth2Client(services, configuration);

        return services;
    }

    private static void RegisterSimpleOAuth2Client(IServiceCollection services, ClientCredentialConfig configuration)
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
            .AddSingleton<IAuthorizationGrant, ClientCredentials>(serviceProvider =>
            {
                IHttpClientFactory httpClientFactory = serviceProvider
                    .GetRequiredService<IHttpClientFactory>();

                IAuthorizationServerErrorHandler httpResponseMessageErrorHandler = serviceProvider
                    .GetRequiredService<IAuthorizationServerErrorHandler>();

                return new ClientCredentials(httpClientFactory, httpResponseMessageErrorHandler, configuration);
            })
            .AddSingleton<IOAuthClient, OAuthClient>();
    }

    private static void ValidateClientCredentialWorkflowConfig(ClientCredentialConfig config)
    {
        ClientCredentialConfigValidator clientCredentialWorkflowConfigValidator = new();
        ValidationResult validationResult = clientCredentialWorkflowConfigValidator.Validate(config);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(validationResult.ToString());
        }
    }
}
