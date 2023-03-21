namespace SimpleOAuth2Client.AspNetCore.Options;

/// <summary>
/// The required options for the grant type client_credentials.
/// </summary>
public sealed record ClientCredentialOptions
{
    /// <summary>
    /// The Id of the client.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// The secret of the client.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    public string ClientSecret { get; set; } = null!;

    /// <summary>
    /// The scope of the access request
    /// </summary>
    /// <remarks>OPTIONAL.</remarks>
    public string? Scope { get; set; }

    /// <summary>
    /// The token endpoint provided by the authorization server.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    public Uri TokenEndpoint { get; set; } = null!;
}
