# About SimpleOAuth2Client.AspNetCore
This repository provide a client for ASP.NET Core based projects that supports parts of the OAuth 2.0 Authorization Framework [RFC6749]

# Important Information
At the moment only the authorization grant type "client credentials" is supported. Later the remaining ones are added.

## Install NuGet Package
    PM> Install-Package -Id SimpleOAuth2Client.AspNetCore -Version 1.0.0

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

## Enable Sensible HttpClient logging

It is possible to log the sensible HttpClient data from the AccessToken-Request and AccessToken-Response. By default the logging is disabled. But it can be enabled by the `SimpleOAuth2ClientOptions`:

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure the SimpleOAuth2Client
builder.Services.AddSimpleOAuth2Client(options =>
{
    // Configure the options for the client_credentials grant type
    // ...

    // Enable sensible HttpClient logging
    options.EnableSensibleHttpClientLogging = true;
});
```

The output of a successful client_credentials workflow looks like this:

```
info: SimpleOAuth2Client.AspNetCore.Common.Http.Delegates.LoggingDelegate[1000]
      HTTP-Request General Information: Method: POST, RequestUri: 'https://localhost:5002/connect/token', Version: 1.1, Content: System.Net.Http.FormUrlEncodedContent, Headers:
      {
        Authorization: Basic dGVzdC1jbGllbnQ6cGFzc3dvcmQxMjM0NQ==
        Accept: application/json
        Content-Type: application/x-www-form-urlencoded
      }
info: SimpleOAuth2Client.AspNetCore.Common.Http.Delegates.LoggingDelegate[1000]
      HTTP-Request Content: grant_type=client_credentials
info: System.Net.Http.HttpClient.AuthClient.ClientHandler[100]
      Sending HTTP request POST https://localhost:5002/connect/token
info: System.Net.Http.HttpClient.AuthClient.ClientHandler[101]
      Received HTTP response headers after 386.1274ms - 200
info: SimpleOAuth2Client.AspNetCore.Common.Http.Delegates.LoggingDelegate[1001]
      HTTP-Response General Information: StatusCode: 200, ReasonPhrase: 'OK', Version: 1.1, Content: System.Net.Http.HttpConnectionResponseContent, Headers:
      {
        Date: Wed, 12 Apr 2023 06:10:03 GMT
        Server: Kestrel
        Cache-Control: no-store, no-cache, max-age=0
        Pragma: no-cache
        Transfer-Encoding: chunked
        Content-Type: application/json; charset=UTF-8
      }
info: SimpleOAuth2Client.AspNetCore.Common.Http.Delegates.LoggingDelegate[1001]
      HTTP-Response Content: 
      {
        "access_token":"XXXXXX",
        "expires_in":3600,
        "token_type":"Bearer",
        "scope":"test-api"
     }
```

## How to Debug the NuGet-Package

First of all we need to configure Visual Studio 2022 to use Source Link. The related Symbol files are uploaded to the Symbol File repository of NuGet.org.

The first step is to enable the download form the symbol files (Tools --> Options --> Debugging --> Symbols)

![Vs_Symbols](https://user-images.githubusercontent.com/9673822/228032174-d48ee8d6-5471-4e0c-89ba-8eb33b624ced.PNG)

Next step is to disable “Enable Just My Code” option (Tools --> Options --> Debugging --> General)

![Vs_DisbaleMyCode](https://user-images.githubusercontent.com/9673822/228032829-35795e0d-e400-493b-a98b-23e067cfa310.PNG)

The last step is to enable Source Server support (Tools --> Options --> Debugging --> General)

![Vs_SourceServerSupport](https://user-images.githubusercontent.com/9673822/228033588-822c7184-ec33-4cd4-8c26-5b6b1d2937bd.PNG)

After the successful configuration of Visual Studio 2022 we can set a Breakpoint and start debugging

![Vs_Breakpoint](https://user-images.githubusercontent.com/9673822/228034472-a57d24ac-c071-443d-880a-28bdc99a3a22.PNG)

Hint: The first application startup take some time, because Visual Studio try to download the symbol files!
