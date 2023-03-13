# About SimpleOAuth2Client.AspNetCore
This repository provide a client for ASP.NET Core based projects that supports parts of the OAuth 2.0 Authorization Framework [RFC6749]

# Important Information
At the moment only the authorization grant type "client credentials" is supported. Later the remaining ones are added.

## NuGet
    PM> Install-Package SimpleOAuth2Client.AspNetCore

## How?
The SimpleOAuth2Client project defines a service interface named `IOAuth2Client` to request access tokens from a OAuth2 based authorization server. The services is registred with lifetime "Signleton" and can be injected via the ASP.NET Core IoC-Container.

## Usage

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure the SimpleOAuth2Client
builder.Services.AddSimpleOAuth2Client(options =>
{
    options.ClientId = <ClientId>;
    options.ClientSecret = <ClientSecret>;
    options.TokenEndpoint = <TokenEndpoint>;
});

WebApplication app = builder.Build();

// Inject and use the IOAuth2Client as a service
app.MapGet("/test-access-token-request", async (IOAuth2Client client) =>
{
    Result<AccessToken, OAuth2Error> result = await client.RequestAccessToken();
    if (result.IsFailure)
    {
        return Results.BadRequest(result.Error);
    }

    return Results.Ok(result.Value);
});

app.Run();
```
