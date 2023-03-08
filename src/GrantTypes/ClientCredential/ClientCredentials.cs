using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Common.Http;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.Options;

namespace SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;

/// <summary>
/// The implementation of the IAuthorizationGrant interface.
/// </summary>
internal sealed class ClientCredentials : IAuthorizationGrant
{
    private readonly IAuthorizationServerErrorHandler _authorizationServerErrorHandler;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<ClientCredentialOptions> _options;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClientfactor to create named clients.</param>
    /// <param name="authorizationServerErrorHandler">The AuthorizationServerErrorHandler.</param>
    /// <param name="options">The options for ClientCredentials grant type.</param>
    /// <exception cref="ArgumentNullException">If one of the constructor parameters are null.</exception>
    public ClientCredentials(
        IHttpClientFactory httpClientFactory,
        IAuthorizationServerErrorHandler authorizationServerErrorHandler,
        IOptionsMonitor<ClientCredentialOptions> options)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _authorizationServerErrorHandler = authorizationServerErrorHandler ?? throw new ArgumentNullException(nameof(authorizationServerErrorHandler));
    }

    /// <inheritdoc/>
    public async Task<Result<AccessToken, OAuth2Error>> Execute()
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient("AuthClient");

        using HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, _options.CurrentValue.TokenEndpoint)
        {
            Content = new AccessTokenRequest("client_credentials", _options.CurrentValue.Scope).HttpContent,
        };

        string username = _options.CurrentValue.ClientId;
        string password = _options.CurrentValue.ClientSecret;

        httpRequestMessage.Headers.Authorization = new BasicAuthenticationHeaderValue(username, password);

        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return await _authorizationServerErrorHandler.HandleAuthorizationServerError(httpResponseMessage);
        }

        return await ParseAccessToken(httpResponseMessage);
    }

    private static async Task<Result<AccessToken, OAuth2Error>> ParseAccessToken(HttpResponseMessage httpResponseMessage)
    {
        string httpResponseMessageContent = await httpResponseMessage.Content.ReadAsStringAsync();
        AccessTokenResponse? accessTokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(httpResponseMessageContent);

        return accessTokenResponse is not null ?
            new AccessToken(accessTokenResponse.AccessToken, accessTokenResponse.ExpiresIn) : OAuth2Errors.AccessTokenResponse(httpResponseMessageContent);
    }
}
