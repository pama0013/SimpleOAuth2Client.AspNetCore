using SimpleOAuth2Client.AspNetCore.Common.Errors;

namespace SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;

/// <summary>
/// Interface to handle errors from the authorization server for different grant types.
/// </summary>
internal interface IAuthorizationServerErrorHandler
{
    /// <summary>
    /// Handle errors from the authorization server.
    /// </summary>
    /// <param name="httpResponseMessage">The HttpResponseMessage from the authorization server.</param>
    /// <returns>The related OAuth2 error.</returns>
    Task<OAuth2Error> HandleAuthorizationServerError(HttpResponseMessage httpResponseMessage);
}
