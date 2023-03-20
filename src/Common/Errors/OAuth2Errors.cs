namespace SimpleOAuth2Client.AspNetCore.Common.Errors;

/// <summary>
/// Provide some convenience methods to create errors.
/// </summary>
internal static class OAuth2Errors
{
    private const string AccessTokenRequestErrorCode = "OAuth2.AccessTokenRequest";
    private const string AccessTokenResponseErrorCode = "OAuth2.AccessTokenResponse";
    private const string AuthorizationServerErrorCode = "OAuth2.AuthorizationServer";
    private const string UnhandledErrorCode = "OAuth2.Unhandled";

    /// <summary>
    /// Provide a error that is related to the access token request.
    /// </summary>
    /// <param name="errorDescription">The error description.</param>
    /// <returns>The error.</returns>
    public static OAuth2Error AccessTokenRequest(string errorDescription) => new(AccessTokenRequestErrorCode, errorDescription);

    /// <summary>
    /// Provide a error that is related to the access token response.
    /// </summary>
    /// <param name="errorDescription">The error description.</param>
    /// <returns>The error.</returns>
    public static OAuth2Error AccessTokenResponse(string errorDescription) => new(AccessTokenResponseErrorCode, errorDescription);

    /// <summary>
    /// Provide a error that is related to the authorization server.
    /// </summary>
    /// <param name="errorDescription">The error description.</param>
    /// <returns>The error.</returns>
    public static OAuth2Error AuthorizationServer(string errorDescription) => new(AuthorizationServerErrorCode, errorDescription);

    /// <summary>
    /// Provide a error that is not handled by the application.
    /// </summary>
    /// <param name="errorDescription">The error description.</param>
    /// <returns>The error.</returns>
    public static OAuth2Error Unhandled(string errorDescription) => new(UnhandledErrorCode, errorDescription);
}
