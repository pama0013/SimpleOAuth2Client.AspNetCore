using CSharpFunctionalExtensions;

using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.Example.Net6.Options;
using SimpleOAuth2Client.AspNetCore.Extensions;
using SimpleOAuth2Client.AspNetCore.Model;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Pass the SimpleOAuth2ClientOptions as UserSecrets,Environment variables or command line arguments to the application
IConfigurationSection section = builder.Configuration.GetSection("SimpleOAuth2ClientOptions");
SimpleOAuth2ClientOptions? simpleOAuth2ClientOptions = section
    .Get<SimpleOAuth2ClientOptions>()
    ?? throw new InvalidOperationException("Could not load SimpleOAuth2ClientOptions.");

builder.Services.AddSimpleOAuth2Client(options =>
{
    // Configure the options for the client_credentials grant type
    options.ClientCredentialOptions.ClientId = simpleOAuth2ClientOptions.ClientId;
    options.ClientCredentialOptions.ClientSecret = simpleOAuth2ClientOptions.ClientSecret;
    options.ClientCredentialOptions.TokenEndpoint = simpleOAuth2ClientOptions.TokenEndpoint;

    // Configure the options for the retry algorithm
    options.RetryOptions.TimeoutPerRetry = 15;
    options.RetryOptions.RetryAttempts = 3;
    options.RetryOptions.FirstRetryDelay = 3;

    // Disable ServerCertificateValidation for development
    options.DisableServerCertificateValidation = true;

    // Enable sensible HttpClient logging
    options.EnableSensibleHttpClientLogging = true;
});

WebApplication app = builder.Build();

app.MapGet("/test-access-token", async (IOAuth2Client client) =>
{
    Result<AccessToken, OAuth2Error> result = await client.RequestAccessToken();
    return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
});

app.Run();
