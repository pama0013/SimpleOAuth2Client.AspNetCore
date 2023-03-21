using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

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

        services
            .AddHttpClient("AuthClient", httpClient =>
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            })
            .AddHttpMessageHandler<TransientErrorHandlerDelegate>()
            .AddCustomizedHttpClientPolicies();

        return services;
    }

    /// <summary>
    /// Add Polly based customized HttpClient policies.
    /// </summary>
    /// <param name="httpClient">The HttpClientBuilder to modify the named HttpClient.</param>
    /// <returns>The modified HttpClientBuilder.</returns>
    private static IHttpClientBuilder AddCustomizedHttpClientPolicies(this IHttpClientBuilder httpClient)
    {
        // Define a timeout policy per request of 10 seconds
        AsyncTimeoutPolicy<HttpResponseMessage> timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);

        // Define a retry policy
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(2), 5));

        httpClient
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        return httpClient;
    }
}
