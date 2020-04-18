using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static async Task Main()
        {
            var resultado = await RequestWithPolicy(async (client, disco) =>
                await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "cliente_iya",
                    ClientSecret = "123654"
                }));

            Console.WriteLine(resultado);

            resultado = await RequestWithPolicy(async (client, disco) =>
            {
                string clientId = "ro.cliente_iya";

                return await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientId,
                    ClientSecret = "123654",
                    UserName = "bob",
                    Password = "bob"
                });
            });

            Console.WriteLine(resultado);

            Console.ReadLine();
        }


        private static async Task<string> RequestWithPolicy(Func<HttpClient, DiscoveryDocumentResponse, Task<TokenResponse>> getTokenResponse)
        {
            async Task<string> GetAccessToken()
            {
                // discover endpoints from metadata
                var client = new HttpClient();

                var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
                {
                    Policy = new DiscoveryPolicy { RequireHttps = false },
                    Address = "http://kronos:5010"
                });

                if (disco.IsError)
                {
                    Console.WriteLine(disco.Error);
                    return disco.Error;
                }

                // request token
                var tokenResponse = await getTokenResponse(client, disco);

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return tokenResponse.Error;
                }

                Console.WriteLine(tokenResponse.Json);
                Console.WriteLine("\n\n");

                return tokenResponse.AccessToken;
            }

            using (var apiClient = new HttpClient())
            {
                var accessToken = await GetAccessToken();

                // call api
                apiClient.SetBearerToken(accessToken);

                var response = await apiClient.GetAsync("http://localhost:5001/api/identity");
                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }
                else
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}