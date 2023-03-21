using System.Net;
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
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<SimpleOAuth2ClientOptions> _options;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="httpClientFactory">The HttpClientfactor to create named clients.</param>
    /// <param name="options">The options for SimpleOAuth2Client.</param>
    /// <exception cref="ArgumentNullException">If one of the constructor parameters are null.</exception>
    public ClientCredentials(IHttpClientFactory httpClientFactory, IOptionsMonitor<SimpleOAuth2ClientOptions> options)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc/>
    public async Task<Result<AccessToken, OAuth2Error>> Execute()
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient("AuthClient");

        Uri tokenEndpoint = _options.CurrentValue.ClientCredentialOptions.TokenEndpoint;
        string? scope = _options.CurrentValue.ClientCredentialOptions.Scope;
        string grantType = "client_credentials";

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new AccessTokenRequest(grantType, scope).HttpContent,
        };

        string username = _options.CurrentValue.ClientCredentialOptions.ClientId;
        string password = _options.CurrentValue.ClientCredentialOptions.ClientSecret;

        httpRequestMessage.Headers.Authorization = new BasicAuthenticationHeaderValue(username, password);

        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return await HandleHttpResponseMessageError(httpResponseMessage);
        }

        return await ParseAccessToken(httpResponseMessage);
    }

    private static async Task<OAuth2Error> HandleHttpResponseMessageError(HttpResponseMessage httpResponseMessage)
    {
        string httpResponseMessageContent = await httpResponseMessage.Content.ReadAsStringAsync();

        return httpResponseMessage.StatusCode switch
        {
            // In case of a HttpStatusCode.Unauthorized --> ClientCredentials are wrong (Authentication failed)
            HttpStatusCode.Unauthorized => OAuth2Errors.AccessTokenRequest("invalid_client"),

            // In case of a HttpStatusCode.InternalServerError --> Authorization server error or transient error
            HttpStatusCode.InternalServerError => OAuth2Errors.AuthorizationServer(httpResponseMessageContent),

            // In case of a HttpStatusCode.BadRequest --> AccessTokenRequest was invalid
            HttpStatusCode.BadRequest => HandleHttpStatusCodeBadRequest(httpResponseMessageContent),

            _ => OAuth2Errors.Unhandled(httpResponseMessageContent)
        };
    }

    private static OAuth2Error HandleHttpStatusCodeBadRequest(string httpResponseMessageContent)
    {
        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(httpResponseMessageContent);

        return errorResponse is null
            ? OAuth2Errors.AccessTokenRequest(httpResponseMessageContent)
            : OAuth2Errors.AccessTokenRequest($"[{errorResponse.Error}|{errorResponse.ErrorDescription ?? "-"}]");
    }

    private static async Task<Result<AccessToken, OAuth2Error>> ParseAccessToken(HttpResponseMessage httpResponseMessage)
    {
        string httpResponseMessageContent = await httpResponseMessage.Content.ReadAsStringAsync();

        AccessTokenResponse? accessTokenResponse = JsonSerializer.Deserialize<AccessTokenResponse>(httpResponseMessageContent);
        if (accessTokenResponse is null)
        {
            return OAuth2Errors.AccessTokenResponse("Could not deserialize AccessTokenResponse");
        }

        return new AccessToken(accessTokenResponse.AccessToken, accessTokenResponse.ExpiresIn);
    }
}
