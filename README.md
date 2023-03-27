# About SimpleOAuth2Client.AspNetCore
This repository provide a client for ASP.NET Core based projects that supports parts of the OAuth 2.0 Authorization Framework [RFC6749]

# Important Information
At the moment only the authorization grant type "client credentials" is supported. Later the remaining ones are added.

## Install NuGet Package
    PM> Install-Package -Id SimpleOAuth2Client.AspNetCore -Version 1.0.0-beta

## How it works?
The SimpleOAuth2Client project defines a service interface named `IOAuth2Client` to request access tokens from a OAuth2 based authorization server. The services is registered with lifetime "Singleton" and can be injected via the ASP.NET Core IoC-Container.

## Usage

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure the SimpleOAuth2Client
builder.Services.AddSimpleOAuth2Client(options =>
{
    // Configure the options for the client_credentials grant type
    options.ClientCredentialOptions.ClientId = <ClientId>;
    options.ClientCredentialOptions.ClientSecret = <ClientSecret>;
    options.ClientCredentialOptions.TokenEndpoint = <TokenEndpoint>;
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

## Retry mechanism

By default the SimpleOAuth2Client use a retry mechanism to handle transient network errors. A client can override the
default values:

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure the SimpleOAuth2Client
builder.Services.AddSimpleOAuth2Client(options =>
{
    // Configure the options for the client_credentials grant type
    // ...

    // Configure the options for the retry algorithm

    // Default value is 10
    options.RetryOptions.TimeoutPerRetry = 15;

    // Default value is 5
    options.RetryOptions.RetryAttempts = 3;

    // Default value is 2
    options.RetryOptions.FirstRetryDelay = 3;
});
```


## Disable Server Certificate validation

For development or testing purpose we can temporally disable the validation of the server certificate:

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure the SimpleOAuth2Client
builder.Services.AddSimpleOAuth2Client(options =>
{
    // Configure the options for the client_credentials grant type
    // ...

    // Disable ServerCertificateValidation for development
    options.DisableServerCertificateValidation = true;
});
```

Please be careful with this feature. Do not use this feature in production!

## How to Debug the NuGet-Package

TBD
