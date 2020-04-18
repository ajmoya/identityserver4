using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using static IdentityServer.Utils.Constantes;

namespace IdentityServer.Utils
{
    public static class Config
    {
        // Si se quiere que un cliente incorpore un claim de usuario en el access token,
        // se debe añadir el ApiResource (o su scope) en su alcance permitido (AllowedScopes)
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                // El ApiResource "api.iya" tiene los UserClaims "rol" y "modulos"
                // Es necesario incluir este ApiResource (o su scope) en el alcance permitido de un cliente,
                // para que se añadan sus UserClaims en el access token
                new ApiResource(ConstApiResources.Iya.Nombre, ConstApiResources.Iya.Descripcion,
                    new[] {ConstClaimTypes.Rol, ConstClaimTypes.Modulos})
                {
                    Scopes =
                    {
                        new Scope(ConstApiResources.Iya.Scope.Read),
                        new Scope(ConstApiResources.Iya.Scope.Write),
                        new Scope(ConstApiResources.Iya.Scope.Full)
                    }
                }
            };

        // Si se quiere que un cliente incorpore un claim de usuario en el identity token,
        // se debe añadir el IdentityResource en su alcance permitido (AllowedScopes)
        public static IEnumerable<IdentityResource> Ids =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // standard OpenID Connect: subject id 
                new IdentityResources.Profile(), // standard OpenID Connect
                new IdentityResources.Email(), // standard OpenID Connect

                // El IdentityResource "perfil" tiene los UserClaims "rol" y "modulos"
                // Es necesario incluir este IdentityResource en el alcance permitido del cliente "mvc",
                // para que se añadan sus UserClaims en el identity token
                new IdentityResource(ConstIdentityResources.Perfil.Nombre, ConstIdentityResources.Perfil.Descripcion,
                    new[] {ConstClaimTypes.Rol, ConstClaimTypes.Modulos})
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // CLIENTES NO INTERACTIVOS => MAQUINA - MAQUINA
                // Usa el clientid/secret para authentication
                new Client
                {
                    ClientId = "cliente_iya",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets = { new Secret("123456".Sha256()) },

                    // scopes that client has access to
                    AllowedScopes = { ConstApiResources.Iya.Scope.Full },

                    ClientClaimsPrefix = "",
                    Claims =
                    {
                        new Claim(ConstClaimTypes.Rol, ConstClaimValueTypes.Admin),
                        new Claim(ConstClaimTypes.Modulos, "recursos, reporte-variables")
                    }
                },

                // Usa el clientid/secret y un user/pass de algún usuario para authentication
                new Client
                {
                    ClientId = "ro.cliente_iya",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("123456".Sha256()) },
                    AllowedScopes = { ConstApiResources.Iya.Scope.Full }
                },

                // CLIENTE INTERACTIVO CON AUTENTICACIÓN PREVIA
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("123456".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        // El cliente puede acceder al recurso API
                        ConstApiResources.Iya.Scope.Full,
                        
                        // El cliente puede acceder a los siguientes recursos de identidad
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        ConstIdentityResources.Perfil.Nombre
                    },

                    AllowOfflineAccess = true,
                    //AlwaysIncludeUserClaimsInIdToken = true
                }
            };

        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                Claims =
                {
                    new Claim(ConstClaimTypes.Rol, "STE"),
                    new Claim(ConstClaimTypes.Modulos, "recursos"),

                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                }
            },
            new TestUser{SubjectId = "999", Username = "bob", Password = "bob",
                Claims =
                {
                    new Claim(ConstClaimTypes.Rol, ConstClaimValueTypes.Admin),
                    new Claim(ConstClaimTypes.Modulos, "config, recursos, variables-reportes, analisis-reporting"),

                    new Claim(JwtClaimTypes.PhoneNumber, "675568524", ClaimValueTypes.String),
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                }
            }
        };
    }
}