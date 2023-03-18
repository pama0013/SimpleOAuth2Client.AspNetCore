# ASP.NET Core 7 Example
This is a simple example of using the SimpleOAuth2Client.AspNetCore NuGet-Package in a ASP.NET Core 7 project with Minimal Api.

# Getting Started
First of all we need to start the minimal implementation of the IdentityServer4 application (Can be found under the path /examples/shared/identityserver4). This server is not configured for production use cases. At the moment only one client for testing is configured.

The client credentials are:
- ClientID = test-client
- ClientSecret = password12345
- TokenEndpoint = https://localhost:5002/connect/token

To handover the SimpleOAuth2ClientOptions to the ASP.NET Core 7 example we have multiple options:
1. Define the client credentials in the appsettings.json file
2. Define the client credentials as environment variables
3. Define the client credentials as command line parameter

Example for appsettings.json:

    "SimpleOAuth2ClientOptions": {
        "ClientId": "test-client",
        "ClientSecret": "password12345",
        "TokenEndpoint": "https://localhost:5002/connect/token"
    },
    
Example for environment variables:

    set SimpleOAuth2ClientOptions__ClientId=test-client
    set SimpleOAuth2ClientOptions__ClientSecret=password12345
    set SimpleOAuth2ClientOptions__TokenEndpoint=https://localhost:5002/connect/token
    
After the IdentityServer4 is started and the required SimpleOAuth2ClientOptions are configured the example application can be started. When the startup process was successfull we can test the MinimalApi endpoint.
