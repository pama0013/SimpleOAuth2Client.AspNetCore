namespace SimpleOAuth2Client.AspNetCore.Options;

/// <summary>
/// The options for the SimpleOAuth2Client.
/// </summary>
public sealed record SimpleOAuth2ClientOptions
{
    /// <summary>
    /// Gets or sets the options for the grant type client_credential.
    /// </summary>
    public ClientCredentialOptions ClientCredentialOptions { get; set; } = new ClientCredentialOptions();

    /// <summary>
    /// Gets or sets the options for the retry handling.
    /// </summary>
    public RetryOptions RetryOptions { set; get; } = new RetryOptions();
}
