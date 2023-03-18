// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add resources for IdentityServer and enable InMemory store for ApiScopes and Clients
builder
    .Services
    .AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

WebApplication app = builder.Build();

// Enable DeveloperExceptionPages if the application is build for development
// See launchSettings.json: "ASPNETCORE_ENVIRONMENT": "Development"
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable IdentityServer middleware
app.UseIdentityServer();

app.Run();