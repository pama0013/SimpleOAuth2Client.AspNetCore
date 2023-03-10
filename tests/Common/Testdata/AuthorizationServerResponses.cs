namespace SimpleOAuth2Client.AspNetCore.UnitTests.Common.Testdata;

/// <summary>
/// Provide some convenience methods to fake the HTTP-Response messages from the authorization server.
/// </summary>
internal static class AuthorizationServerResponses
{
    /// <summary>
    /// Create a valid OAuth2 access token response message.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="tokenType">The token type.</param>
    /// <param name="expiresIn">The number of seconds the access token expires.</param>
    /// <returns>The access token response.</returns>
    public static HttpContent CreateAccessTokenResponse(string accessToken, string tokenType, int expiresIn)
    {
        string accessTokenResponse = $$"""
        {
            "access_token": "{{accessToken}}",
            "expires_in": {{expiresIn}},
            "token_type": "{{tokenType}}"
        }
        """;

        return new StringContent(accessTokenResponse);
    }

    /// <summary>
    /// Create a invalid OAuth2 access token response message.
    /// </summary>
    /// <returns>The access token response.</returns>
    public static HttpContent CreateAccessTokenResponseWithNullValue() => new StringContent("null");

    /// <summary>
    /// Create a valid OAuth2 error response with a optional error description.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <param name="errorDescription">The error description.</param>
    /// <returns>The error response.</returns>
    public static HttpContent CreateErrorResponseWithDescription(string error, string errorDescription)
    {
        string errorResponseWithDescription = $$"""
        {
            "error": "{{error}}",
            "error_description": "{{errorDescription}}"
        }
        """;

        return new StringContent(errorResponseWithDescription);
    }

    /// <summary>
    /// Create a invalid OAuth2 error response.
    /// </summary>
    /// <returns>The error response.<returns>
    public static HttpContent CreateErrorResponseWithNullValue() => new StringContent("null");

    /// <summary>
    /// Create a valid OAuth2 error response without a optional error description.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <returns>The error response.</returns>
    public static HttpContent CreateErrorResponseWithoutDescription(string error)
    {
        string errorResponseWithoutDescription = $$"""
        {
            "error": "{{error}}"
        }
        """;

        return new StringContent(errorResponseWithoutDescription);
    }
}
