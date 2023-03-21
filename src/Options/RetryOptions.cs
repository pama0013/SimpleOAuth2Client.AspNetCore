namespace SimpleOAuth2Client.AspNetCore.Options;

/// <summary>
/// The options for the automatic retry handling.
/// </summary>
public sealed record RetryOptions
{
    /// <summary>
    /// Gets and sets the timeout in seconds for a single retry.
    /// </summary>
    /// <remarks>The default value is 10 seconds.</remarks>
    public int TimeoutPerRetry { get; set; } = 10;

    /// <summary>
    /// Gets and sets the delay in seconds for the first retry.
    /// </summary>
    /// <remarks>The default value is 2 seconds.</remarks>
    public int FirstRetryDelay { get; set; } = 2;

    /// <summary>
    /// Gets and sets the number of retry attempts.
    /// </summary>
    /// <remarks>The default value is 5.</remarks>
    public int RetryAttempts { get; set; } = 5;
}
