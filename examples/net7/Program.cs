using CSharpFunctionalExtensions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.Example.Options;
using SimpleOAuth2Client.AspNetCore.Extensions;
using SimpleOAuth2Client.AspNetCore.Model;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Pass the SimpleOAuth2ClientOptions as command line arguments or environment variables
IConfigurationSection section = builder.Configuration.GetSection("SimpleOAuth2ClientOptions");
SimpleOAuth2ClientOptions? simpleOAuth2ClientOptions = section
    .Get<SimpleOAuth2ClientOptions>()
    ?? throw new InvalidOperationException("Could not load SimpleOAuth2ClientOptions.");

builder.Services.AddSimpleOAuth2Client(options =>
{
    options.ClientId = simpleOAuth2ClientOptions.ClientId;
    options.ClientSecret = simpleOAuth2ClientOptions.ClientSecret;
    options.TokenEndpoint = simpleOAuth2ClientOptions.TokenEndpoint;
});

WebApplication app = builder.Build();

app.MapGet("/test-access-token", async (IOAuth2Client client) =>
{
    Result<AccessToken, OAuth2Error> result = await client.RequestAccessToken();
    if (result.IsFailure)
    {
        return Results.BadRequest(result.Error);
    }

    return Results.Ok(result.Value);
});

app.Run();
