using Microsoft.Extensions.Logging;

namespace SimpleOAuth2Client.AspNetCore.Common.Http.Delegates;

/// <summary>
/// This class logs HTTP related information.
/// </summary>
/// <remarks>Logs all information related HttpRequestMessage and HttpResponseMessage objects.</remarks>
internal sealed class LoggingDelegate : DelegatingHandler
{
    private readonly ILogger<LoggingDelegate> _logger;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="logger">The logger instance for this class.</param>
    /// <exception cref="ArgumentNullException">If one of the parameter is null.</exception>
    public LoggingDelegate(ILogger<LoggingDelegate> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await LogHttpRequestMessage(request);

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        await LogHttpResponseMessage(response);

        return response;
    }

    private async Task LogHttpRequestMessage(HttpRequestMessage httpRequestMessage)
    {
        _logger.LogInformation(HttpClientLogEvents.HttpRequestMessage, "HTTP-Request General Information: {Request}", httpRequestMessage);

        if (httpRequestMessage.Content is not null)
        {
            string content = await httpRequestMessage.Content.ReadAsStringAsync();
            _logger.LogInformation(HttpClientLogEvents.HttpRequestMessage, "HTTP-Request Content: {Content}", content);
        }
    }

    private async Task LogHttpResponseMessage(HttpResponseMessage httpResponseMessage)
    {
        _logger.LogInformation(HttpClientLogEvents.HttpResponseMessage, "HTTP-Response General Information: {Response}", httpResponseMessage);

        string content = await httpResponseMessage.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(content))
        {
            _logger.LogInformation(HttpClientLogEvents.HttpResponseMessage, "HTTP-Response Content: {Content}", content);
        }
    }

    /// <summary>
    /// EventId's for logging.
    /// </summary>
    private static class HttpClientLogEvents
    {
        /// <summary>
        /// EventId for HttpRequestMessage.
        /// </summary>
        public const int HttpRequestMessage = 1000;

        /// <summary>
        /// EventId for HttpRepsoneMessage.
        /// </summary>
        public const int HttpResponseMessage = 1001;
    }
}
