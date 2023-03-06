using CSharpFunctionalExtensions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Model;

namespace SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;

/// <summary>
/// Interface to define a way to execute one of the OAuth2 supported authorization grants.
/// </summary>
internal interface IAuthorizationGrant
{
    /// <summary>
    /// Execute the authorization grant type.
    /// </summary>
    /// <returns>If the execution was successful the AccessToken is returned. Otherwise a OAuth2Error.</returns>
    Task<Result<AccessToken, OAuth2Error>> Execute();
}
