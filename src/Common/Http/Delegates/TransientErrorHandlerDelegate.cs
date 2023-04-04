using System.Net;
using Microsoft.Extensions.Logging;
using Polly.Timeout;

namespace SimpleOAuth2Client.AspNetCore.Common.Http.Delegates;

/// <summary>
/// This class handles exceptions which are related to transient network errors.
/// </summary>
internal sealed class TransientErrorHandlerDelegate : DelegatingHandler
{
    private readonly ILogger<TransientErrorHandlerDelegate> _logger;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="logger">The logger instance for this class.</param>
    /// <exception cref="ArgumentNullException">If one of the parameter is null.</exception>
    public TransientErrorHandlerDelegate(ILogger<TransientErrorHandlerDelegate> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is TimeoutRejectedException)
        {
            _logger.LogError(HttpClientLogEvents.SendAsync, ex, "A exception was thrown within the HttpClient execution chain.");

            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("The authorization server is currently not available.")
            };
        }
    }

    /// <summary>
    /// EventId's for logging.
    /// </summary>
    private static class HttpClientLogEvents
    {
        /// <summary>
        /// EventId for SendAsync.
        /// </summary>
        public const int SendAsync = 1002;
    }
}
