namespace SimpleOAuth2Client.AspNetCore.Example.Net7.Options;

public sealed class SimpleOAuth2ClientOptions
{
    /// <summary>
    /// The Id of the client.
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// The secret of the client.
    /// </summary>
    public string ClientSecret { get; set; } = null!;

    /// <summary>
    /// The scope of the access request
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// The token endpoint provided by the authorization server.
    /// </summary>
    public Uri TokenEndpoint { get; set; } = null!;
}
