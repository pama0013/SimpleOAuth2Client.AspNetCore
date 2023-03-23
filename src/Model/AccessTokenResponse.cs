using System.Text.Json.Serialization;

namespace SimpleOAuth2Client.AspNetCore.Model;

/// <summary>
/// The AccessTokenResponse from the authorization server.
/// </summary>
internal sealed record AccessTokenResponse
{
    /// <summary>
    /// Gets the access token issued by the authorization server.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; }

    /// <summary>
    /// Gets the lifetime in seconds of the access token.
    /// </summary>
    /// <remarks>RECOMMENDED.</remarks>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; }

    /// <summary>
    /// Gets the refresh token, which can be used to obtain new access tokens using the same authorization grant.
    /// </summary>
    /// <remarks>OPTIONAL.</remarks>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; }

    /// <summary>
    /// The scope of the access token.
    /// </summary>
    /// <remarks>OPTIONAL --> if identical to the scope requested by the client; Otherwise --> REQUIRED.</remarks>
    [JsonPropertyName("scope")]
    public string? Scope { get; }

    /// <summary>
    /// Gets the type of the token. The values is case insensitive.
    /// </summary>
    /// <remarks>REQUIRED.</remarks>
    [JsonPropertyName("token_type")]
    public string TokenType { get; }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="accessToken">The access token issued by the authorization server</param>
    /// <param name="tokenType">The type of the token</param>
    /// <param name="expiresIn">The lifetime in seconds of the access token</param>
    /// <param name="refreshToken">The refresh token, which can be used to obtain new access tokens using the same authorization grant</param>
    /// <param name="scope">The scope of the access token</param>
    [JsonConstructor]
    public AccessTokenResponse(string accessToken, string tokenType, int expiresIn, string? refreshToken = null, string? scope = null)
    {
        AccessToken = accessToken;
        ExpiresIn = expiresIn;
        TokenType = tokenType;
        RefreshToken = refreshToken;
        Scope = scope;
    }
}
