using System.Text.Json;
using CSharpFunctionalExtensions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Common.Http;
using SimpleOAuth2Client.AspNetCore.Configuration;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;

namespace SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;

/// <summary>
/// The implementation of the IAuthorizationGrant interface.
/// </summary>
internal sealed class ClientCredentials : IAuthorizationGrant
{
    private readonly IAuthorizationServerErrorHandler _authorizationServerErrorHandler;
    private readonly ClientCredentialConfig _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClientfactor to create named clients.</param>
    /// <param name="authorizationServerErrorHandler">The AuthorizationServerErrorHandler.</param>
    /// <param name="configuration">The configuration for the ClientCredentials grant type.</param>
    /// <exception cref="ArgumentNullException">If one of the constructor parameters are null.</exception>
    public ClientCredentials(
        IHttpClientFactory httpClientFactory,
        IAuthorizationServerErrorHandler authorizationServerErrorHandler,
        ClientCredentialConfig configuration)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _authorizationServerErrorHandler = authorizationServerErrorHandler ?? throw new ArgumentNullException(nameof(authorizationServerErrorHandler));
    }

    /// <inheritdoc/>
    public async Task<Result<AccessToken, OAuth2Error>> Execute()
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient("AuthClient");

        using HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, _configuration.TokenEndpoint)
        {
            Content = new AccessTokenRequest("client_credentials", _configuration.Scope).HttpContent,
        };

        string username = _configuration.ClientId;
        string password = _configuration.ClientSecret;

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
