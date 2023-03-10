using System.Net.Http.Headers;
using System.Text;

namespace SimpleOAuth2Client.AspNetCore.Common.Http;

/// <summary>
/// Provide a HTTP Basic authentication scheme as defined in [RFC2617] to authenticate with the authorization server.
/// </summary>
internal sealed class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="username">The user name to authenticate on the authorization server.</param>
    /// <param name="password">The password to authenticate on the authorization server.</param>
    public BasicAuthenticationHeaderValue(string username, string password)
        : base("Basic", CreateHttpAuthorizationHeader(username, password))
    {
    }

    /// <summary>
    /// Create the value for the Authorization HTTP-Header.
    /// </summary>
    /// <param name="username">The user name to authenticate on the authorization server.</param>
    /// <param name="password">The password to authenticate on the authorization server.</param>
    /// <returns>The value for the Authentication HTTP-Header.</returns>
    private static string CreateHttpAuthorizationHeader(string username, string password)
    {
        string encodedUsername = Encode(username);
        string encodedPassword = Encode(password);

        byte[] utf8EncodedAuthenticationHeader = Encoding.UTF8.GetBytes($"{encodedUsername}:{encodedPassword}");

        return Convert.ToBase64String(utf8EncodedAuthenticationHeader);
    }

    private static string Encode(string data) => Uri.EscapeDataString(data).Replace("%20", "+", StringComparison.InvariantCulture);
}
