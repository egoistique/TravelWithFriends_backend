namespace Travel.Identity.Configuration;

using Travel.Common.Security;
using Duende.IdentityServer.Models;
using IdentityModel;

public static class AppClients
{
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                ClientSecrets =
                {
                    new Secret("A3F0811F2E934C4F1114CB693F7D785E".ToSha256())
                },

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AccessTokenLifetime = 86400, // 1 hour

                AllowedScopes = {
                    AppScopes.TripsRead,
                    AppScopes.TripsWrite
                }
            }
            ,
            new Client
            {
                ClientId = "frontend",
                ClientSecrets =
                {
                    new Secret("A3F0811F2E934C4F1114CB693F7D785E".ToSha256())
                },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                AllowOfflineAccess = true,
                AccessTokenType = AccessTokenType.Jwt,

                AccessTokenLifetime = 86400, // 1 hour

                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AbsoluteRefreshTokenLifetime = 2592000, // 30 days
                SlidingRefreshTokenLifetime = 1296000, // 15 days

                AllowedScopes = {
                    AppScopes.TripsRead,
                    AppScopes.TripsWrite
                }
            }
        };
}