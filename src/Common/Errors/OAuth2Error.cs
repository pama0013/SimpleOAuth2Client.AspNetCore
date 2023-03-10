namespace SimpleOAuth2Client.AspNetCore.Common.Errors;

/// <summary>
/// A error that is related to the OAuth 2.0 authorization framework.
/// </summary>
public sealed record OAuth2Error
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string ErrorDescription { get; }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorDescription">The error description.</param>
    public OAuth2Error(string errorCode, string errorDescription)
    {
        ErrorCode = errorCode;
        ErrorDescription = errorDescription;
    }
}
