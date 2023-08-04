using Duende.IdentityServer.Models;
using IdentityModel;

namespace Client.Server.Data;

public static class IdentityConfig
{ 
    public static IEnumerable<Duende.IdentityServer.Models.Client> GetClients()
    {
        return new[]
        {
            new Duende.IdentityServer.Models.Client()
            {
                ClientId = "Client_App",
                ClientSecrets = { new Secret("Client_App".ToSha256()) },
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = { "Client.App" }
            }
        }.ToArray();
    } 
}