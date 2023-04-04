using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using SimpleOAuth2Client.AspNetCore.Common.Http.Delegates;
using SimpleOAuth2Client.AspNetCore.Options;

namespace SimpleOAuth2Client.AspNetCore.Common.Http.Extensions;

/// <summary>
/// Extension methods to add a customized HttpClient to the ASP.NET Core Service Collection.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "At the moment I don't know how to Unit-Test this extension method.")]
internal static class CustomizedHttpClientServiceCollectionExtensions
{
    /// <summary>
    /// Add a customized HttpClient with all related services.
    /// </summary>
    /// <param name="services">The service collection to which customized HttpClient is added.</param>
    /// <returns>The modified service collection.</returns>
    internal static IServiceCollection AddCustomizedHttpClient(this IServiceCollection services)
    {
        services.TryAddScoped<TransientErrorHandlerDelegate>();

        // Create a new ServiceProvider to resolve the already registered SimpleOAuth2ClientOptions
        using ServiceProvider serviceProvider = services.BuildServiceProvider();

        SimpleOAuth2ClientOptions options = serviceProvider
            .GetRequiredService<IOptionsMonitor<SimpleOAuth2ClientOptions>>()
            .CurrentValue;

        services
            .AddHttpClient("AuthClient", httpClient =>
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            })
            .HandleServerCertificateValidation(options)
            .AddHttpMessageHandler<TransientErrorHandlerDelegate>()
            .AddHttpClientLogging(services, options)
            .AddCustomizedHttpClientPolicies(options);

        return services;
    }

    /// <summary>
    /// Add Polly based customized HttpClient policies.
    /// </summary>
    /// <param name="httpClientBuilder">The HttpClientBuilder to modify the named HttpClient.</param>
    /// <param name="options">The SimpleOAuth2Client options.</param>
    /// <returns>The modified HttpClientBuilder.</returns>
    private static IHttpClientBuilder AddCustomizedHttpClientPolicies(this IHttpClientBuilder httpClientBuilder, SimpleOAuth2ClientOptions options)
    {
        int timeoutPerRetry = options.RetryOptions.TimeoutPerRetry;

        // Define a timeout policy per request of 10 seconds
        AsyncTimeoutPolicy<HttpResponseMessage> timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(timeoutPerRetry);

        int firstRetryDelay = options.RetryOptions.FirstRetryDelay;
        int retryAttempts = options.RetryOptions.RetryAttempts;

        // Define a retry policy
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(firstRetryDelay), retryAttempts));

        httpClientBuilder
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        return httpClientBuilder;
    }

    /// <summary>
    /// Add logging to HttpClient if configured.
    /// </summary>
    /// <param name="httpClientBuilder">The HttpClientBuilder to modify the named HttpClient.</param>
    /// <param name="services">The service collection to which customized HttpClient is added.</param>
    /// <param name="options">The SimpleOAuth2Client options.</param>
    /// <returns>The modified HttpClientBuilder.</returns>
    private static IHttpClientBuilder AddHttpClientLogging(this IHttpClientBuilder httpClientBuilder, IServiceCollection services, SimpleOAuth2ClientOptions options)
    {
        if (options.EnableSensibleHttpClientLogging)
        {
            services.TryAddScoped<LoggingDelegate>();
            httpClientBuilder.AddHttpMessageHandler<LoggingDelegate>();
        }

        return httpClientBuilder;
    }

    /// <summary>
    /// Handle the server certificate validation process.
    /// </summary>
    /// <param name="httpClientBuilder">The HttpClientBuilder to modify the named HttpClient.</param>
    /// <param name="options">The SimpleOAuth2Client options.</param>
    /// <returns>The modified HttpClientBuilder.</returns>
    private static IHttpClientBuilder HandleServerCertificateValidation(this IHttpClientBuilder httpClientBuilder, SimpleOAuth2ClientOptions options)
    {
        if (options.DisableServerCertificateValidation)
        {
            static bool DisableCertificateValidation(HttpRequestMessage _1, X509Certificate2? _2, X509Chain? _3, SslPolicyErrors _4)
                => true;

            httpClientBuilder
                .ConfigureHttpMessageHandlerBuilder(builder => builder.PrimaryHandler = new HttpClientHandler
                {
                    // ############################################################################################################
                    // !! Important Security Information !!
                    // ############################################################################################################

                    // The HttpClient throws a exception in case of SelfSigned certificates.They are not reliable.
                    // As a result we need way to disable the server certificate validation process for testing and development
                    // This way is only valid for known Endpoints and no security relevant use cases!!

                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = DisableCertificateValidation,

                    // ############################################################################################################
                });
        }

        return httpClientBuilder;
    }
}
