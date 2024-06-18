namespace Travel.API.Controllers;

using AutoMapper;
using Travel.Services.UserAccount;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Product")]
[Route("v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="clientFactory">The HTTP client factory instance.</param>
    public AuthController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// Authenticates a user and returns an access token.
    /// </summary>
    /// <param name="request">The login request model containing username and password.</param>
    /// <returns>An authentication result containing the access token and admin status.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Authorize user", Description = "Authenticates a user and returns an access token.")]
    [SwaggerResponse(200, "Success", typeof(AuthResult))]
    [SwaggerResponse(400, "Bad Request", typeof(string))]
    [SwaggerResponse(401, "Unauthorized", typeof(string))]
    [SwaggerResponse(500, "Internal Server Error", typeof(string))]
    public async Task<IActionResult> AuthenticateAsync([FromBody] LoginRequest request)
    {
        try
        {
            var client = _clientFactory.CreateClient();

            //var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7255/connect/token"); 
            //var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "http://host.docker.internal:10001/connect/token");
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "http://travel_identity:8080/connect/token");
            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", request.Username),
                    new KeyValuePair<string, string>("password", request.Password),
                    new KeyValuePair<string, string>("scope", "trips_read trips_write"),
                });


            var clientId = "frontend";
            var clientSecret = "A3F0811F2E934C4F1114CB693F7D785E";
            var credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            tokenRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var tokenResponse = await client.SendAsync(tokenRequest);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                return BadRequest("Authentication failed.");
            }

            var responseContent = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Response Content: " + responseContent); 
            var tokenContent = JsonSerializer.Deserialize<TokenResponse>(responseContent);

            bool isAdmin = request.Username == "admin@travelwf.com";

            Console.WriteLine(tokenContent.AccessToken);

            return Ok(new AuthResult { AccessToken = tokenContent.AccessToken, IsAdmin = isAdmin, });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}



/// <summary>
/// Result of the authentication process.
/// </summary>
public class AuthResult
{
    /// <summary>
    /// Indicates if the user is an admin.
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// The access token.
    /// </summary>
    public string AccessToken { get; set; }
}


/// <summary>
/// Model for login request.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The user email.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The password.
    /// </summary>
    public string Password { get; set; }
}

/// <summary>
/// Model for token response.
/// </summary>
public class TokenResponse
{
    /// <summary>
    /// The access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// The expiration time of the token in seconds.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// The type of the token.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// The scope of the token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}




