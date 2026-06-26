using BL.Common;
using BL.DTOs.Auth.Responses;
using UI.Endpoints;

namespace UI.Services.Auth;

public sealed class ApiTokenRefresher
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiTokenRefresher> _logger;

    public ApiTokenRefresher(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ApiTokenRefresher> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<TokenResponseDto>?> RefreshAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        try
        {
            // Use a raw HttpClient — NOT GenericApiClient — to avoid a circular dependency:
            // GenericApiClient → ITokenService → IApiTokenRefresher → GenericApiClient ❌
            var client = _httpClientFactory.CreateClient("ApiClient");

            var baseUrl = _configuration["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

            client.BaseAddress = new Uri(baseUrl);

            var payload = new { RefreshToken = refreshToken };
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(payload),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(stAuthEndpoints.RefreshAccessToken, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "RefreshToken endpoint returned {StatusCode}. Token is likely expired or invalid.",
                    (int)response.StatusCode);
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<TokenResponseDto>>(
                responseBody,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });


            if (tokenResponse is null || string.IsNullOrWhiteSpace(tokenResponse.Data?.AccessToken))
            {
                _logger.LogWarning("RefreshToken endpoint returned an empty or malformed token response.");
                return null;
            }

            return tokenResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while calling the RefreshToken API endpoint.");
            return null;
        }
    }
}