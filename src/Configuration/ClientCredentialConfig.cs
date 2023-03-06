namespace SimpleOAuth2Client.AspNetCore.Configuration;

/// <summary>
/// The configuration to authenticate a client at the authorization server and request a access token with grant type client_credentials.
/// </summary>
public sealed record ClientCredentialConfig
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
    public string? Scope { set; get; }

    /// <summary>
    /// The token endpoint provided by the authorization server.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    public Uri TokenEndpoint { get; set; } = null!;
}
