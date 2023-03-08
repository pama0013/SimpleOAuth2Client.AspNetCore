using System.Text.Json.Serialization;

namespace SimpleOAuth2Client.AspNetCore.Model;

/// <summary>
///  The error response from the authorization server.
/// </summary>
/// <remarks>In case of a error the authorization server responds with an HTTP 400 (Bad Request).</remarks>
internal sealed record ErrorResponse
{
    /// <summary>
    /// Gets a single ASCII error code from authorization server.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    [JsonPropertyName("error")]
    public string Error { get; }

    /// <summary>
    /// Gets a Human-readable ASCII text providing additional information.
    /// </summary>
    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; }

    /// <summary>
    /// Gets a URI identifying a human-readable web page with information about the error
    /// </summary>
    [JsonPropertyName("error_uri")]
    public Uri? ErrorUri { get; }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="error">The error code from the authorization server.</param>
    /// <param name="errorDescription">The error description from the authorization server.</param>
    /// <param name="errorUri">The error Uri from the authorization server.</param>
    [JsonConstructor]
    public ErrorResponse(string error, string? errorDescription, Uri? errorUri)
    {
        Error = error;
        ErrorDescription = errorDescription;
        ErrorUri = errorUri;
    }
}
