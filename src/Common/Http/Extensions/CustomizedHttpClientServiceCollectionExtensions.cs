using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            .AddHttpMessageHandler<TransientErrorHandlerDelegate>();

        return services;
    }
}
