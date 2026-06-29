using BL.Common;
using BL.DTOs.Auth.Requests;
using BL.DTOs.Auth.Responses;
using Newtonsoft.Json;
using System.Text;
using UI.Endpoints;
using UI.Services.Contracts;

namespace UI.Services.Token;

public class TokenRefreshService : ITokenRefreshService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenProvider _refreshTokenProvider;

    public TokenRefreshService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ITokenProvider tokenProvider,
        IRefreshTokenProvider refreshTokenProvider)
    {
        _httpClient = httpClientFactory.CreateClient("ApiClient");

        _httpClient.BaseAddress = new Uri(
            configuration["ApiSettings:BaseUrl"]
            ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured."));

        _tokenProvider = tokenProvider;
        _refreshTokenProvider = refreshTokenProvider;
    }

    public async Task<bool> RefreshAccessTokenAsync()
    {
        var refreshToken = _refreshTokenProvider.GetRefreshToken();

        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var response = await PostAsync<RefreshAccessTokenResponseDto>(
            stAuthEndpoints.RefreshAccessToken,
            new RefreshTokenRequestDto
            {
                RefreshToken = refreshToken
            });

        if (!response.Success || response.Data is null)
            return false;

        _tokenProvider.SetAccessToken(response.Data.AccessToken);

        return true;
    }

    public async Task<bool> RotateRefreshTokenAsync()
    {
        var refreshToken = _refreshTokenProvider.GetRefreshToken();

        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var response = await PostAsync<RefreshTokenResponseDto>(
            stAuthEndpoints.RotateRefreshToken,
            new RefreshTokenRequestDto
            {
                RefreshToken = refreshToken
            });

        if (!response.Success || response.Data is null)
            return false;

        _tokenProvider.SetAccessToken(response.Data.AccessToken);

        _refreshTokenProvider.SetRefreshToken(
            response.Data.RefreshToken,
            DateTime.UtcNow.AddDays(7));

        return true;
    }

    #region Helpers

    private async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
    {
        var content = new StringContent(
            JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(endpoint, content);

        var json = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(json);

        return apiResponse ?? ApiResponse<T>.InternalServerError("Invalid API response.");
    }

    #endregion
}