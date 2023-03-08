namespace SimpleOAuth2Client.AspNetCore.Model;

/// <summary>
/// This class encapsulate a access token for authorization.
/// </summary>
public sealed record AccessToken
{
    /// <summary>
    /// The creation time of the access token.
    /// </summary>
    private readonly DateTime _created;

    /// <summary>
    /// The expire time of the access token.
    /// </summary>
    private readonly int _expiresIn;

    /// <summary>
    /// Gets the content of the access token.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the information if the access token is expire or not.
    /// </summary>
    public bool IsValid => _created.AddSeconds(_expiresIn) > DateTime.Now;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="expiresIn">The expire time of the access token.</param>
    public AccessToken(string accessToken, int expiresIn)
    {
        Value = accessToken;
        _expiresIn = expiresIn;
        _created = DateTime.Now;
    }
}
