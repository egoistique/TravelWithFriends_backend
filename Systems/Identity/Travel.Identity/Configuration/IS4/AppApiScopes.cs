namespace Travel.Identity.Configuration;

using Travel.Common.Security;
using Duende.IdentityServer.Models;

public static class AppApiScopes
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope(AppScopes.TripsRead, "Read"),
            new ApiScope(AppScopes.TripsWrite, "Write")
        };
}