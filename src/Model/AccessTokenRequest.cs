namespace SimpleOAuth2Client.AspNetCore.Model;

/// <summary>
/// The AccessTokenRequest to the authorization server.
/// </summary>
internal sealed class AccessTokenRequest
{
    private const string OAuth2GrantTypeKey = "grant_type";
    private const string OAuth2ScopeKey = "scope";

    /// <summary>
    /// The content for the request to the token endpoint.
    /// </summary>
    /// <remarks>The content must be using the "application/x-www-form-urlencoded" format</remarks>
    public HttpContent HttpContent { get; }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="grantType">The grant type</param>
    /// <param name="scope">The scope of the access request.</param>
    public AccessTokenRequest(string grantType, string? scope = null)
    {
        var httpContentValues = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>(OAuth2GrantTypeKey, grantType),
        };

        if (scope is not null)
        {
            httpContentValues.Add(new KeyValuePair<string, string>(OAuth2ScopeKey, scope));
        }

        HttpContent = new FormUrlEncodedContent(httpContentValues);
    }
}
