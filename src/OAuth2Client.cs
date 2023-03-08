using CSharpFunctionalExtensions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;

namespace SimpleOAuth2Client.AspNetCore;

/// <summary>
/// The implementation of the IOAuth2Client interface.
/// </summary>
internal sealed class OAuth2Client : IOAuth2Client
{
    private readonly IAuthorizationGrant _authorizationGrant;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="authorizationGrant">The authorization grant.</param>
    /// <exception cref="ArgumentNullException">In case of a parameter is null.</exception>
    public OAuth2Client(IAuthorizationGrant authorizationGrant)
    {
        _authorizationGrant = authorizationGrant;
    }

    /// <inheritdoc/>
    public async Task<Result<AccessToken, OAuth2Error>> RequestAccessToken() => await _authorizationGrant.Execute();
}
