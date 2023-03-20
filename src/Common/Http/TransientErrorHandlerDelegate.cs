using System.Net;

namespace SimpleOAuth2Client.AspNetCore.Common.Http;

/// <summary>
/// This class handles exceptions which are related to transient network errors.
/// </summary>
internal sealed class TransientErrorHandlerDelegate : DelegatingHandler
{
    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
        {
            return new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("The authorization server is currently not available.")
            };
        }
    }
}
