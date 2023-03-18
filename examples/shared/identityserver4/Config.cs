// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;

namespace IdentityServer;

/// <summary>
/// For simplicity a “code as configuration” approach is used.
/// </summary>
public static class Config
{
    /// <summary>
    /// Define the ApiScopes.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new ApiScope("test-api", "My Test-API"),
    };

    /// <summary>
    /// Define the Clients.
    /// </summary>
    public static IEnumerable<Client> Clients => new List<Client>
    {
        new Client
        {
            // The ClientId --> Can be compared with a user name.
            ClientId = "test-client",

            // No interactive user is required. Use clientId + clientSecret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // Define the ClientSecret for authentication --> Can be compared with a password.
            ClientSecrets =
            {
                new Secret("password12345".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "test-api" }
        },
    };

    /// <summary>
    /// Define a standard IdentityResource.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
        new IdentityResources.OpenId(),
    };
}