using CSharpFunctionalExtensions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Model;

namespace SimpleOAuth2Client.AspNetCore.Contracts;

/// <summary>
/// Interface to request a access token from the authorization server.
/// </summary>
public interface IOAuthClient
{
    /// <summary>
    /// Request the access token from the authorization server.
    /// </summary>
    /// <returns>If the request was successful the AccessToken. Otherwise a OAuth2Error.</returns>
    Task<Result<AccessToken, OAuth2Error>> RequestAccessToken();
}
