using System.Text.Json;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;

namespace SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;

/// <summary>
/// The implementation of the IAuthorizationServerErrorHandler interface.
/// </summary>
internal sealed class ClientCredentialsErrorHandler : IAuthorizationServerErrorHandler
{
    public ClientCredentialsErrorHandler()
    {
    }

    public async Task<OAuth2Error> HandleAuthorizationServerError(HttpResponseMessage httpResponseMessage)
    {
        ArgumentNullException.ThrowIfNull(httpResponseMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"No error for HTTP-Status code {httpResponseMessage.StatusCode} available.");
        }

        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return OAuth2Errors.AccessTokenRequest("invalid_client");
        }

        string httpResponseMessageContent = await httpResponseMessage.Content.ReadAsStringAsync();

        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(httpResponseMessageContent);
            if (errorResponse is not null)
            {
                return OAuth2Errors.AccessTokenRequest($"{errorResponse.Error}:{errorResponse.ErrorDescription ?? "-"}");
            }
        }

        return OAuth2Errors.UnhandledError(httpResponseMessageContent);
    }
}
