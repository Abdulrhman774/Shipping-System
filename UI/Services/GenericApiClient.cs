using BL.Common;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using UI.Services.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UI.Services;

/// <summary>
/// A generic HTTP client service that provides a simplified interface for making API calls.
/// This class abstracts the complexity of HttpClient and provides type-safe methods
/// for common HTTP operations (GET, POST, PUT, DELETE).
/// </summary>
public class GenericApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<GenericApiClient> _logger;
    private readonly ITokenRefreshService _tokenRefreshService;
    private readonly IRefreshTokenProvider _refreshTokenProvider;

    public GenericApiClient(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ITokenProvider tokenProvider,
        ILogger<GenericApiClient> logger,
        ITokenRefreshService tokenRefreshService,
        IRefreshTokenProvider refreshTokenProvider)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _tokenProvider = tokenProvider;
        _logger = logger;
        _tokenRefreshService = tokenRefreshService;
        _refreshTokenProvider = refreshTokenProvider;

        var baseUrl = configuration["ApiSettings:BaseUrl"]
            ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }


    /// <summary>
    /// Sends an asynchronous GET request to the specified endpoint and returns ApiResponse{T}.
    /// </summary>
    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await SendAsync(() => _httpClient.GetAsync(endpoint));
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET request failed for {Endpoint}", endpoint);
            return ApiResponse<T>.BadRequest($"Request failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends an asynchronous POST request to the specified endpoint and returns ApiResponse{T}.
    /// </summary>
    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data)
    {
        try
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");

            var response = await SendAsync(() => _httpClient.PostAsync(endpoint, content));
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST request failed for {Endpoint}", endpoint);
            return ApiResponse<T>.BadRequest($"Request failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends an asynchronous PUT request to the specified endpoint and returns ApiResponse { T }.
    /// </summary>
    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");

            var response = await SendAsync(() => _httpClient.PutAsync(endpoint, content));
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT request failed for {Endpoint}", endpoint);
            return ApiResponse<T>.BadRequest($"Request failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends an asynchronous DELETE request without expecting a response body.
    /// </summary>
    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            var response = await SendAsync(() => _httpClient.DeleteAsync(endpoint));
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DELETE request failed for {Endpoint}", endpoint);
            return false;
        }
    }

    public void RemoveAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
        _logger.LogDebug("Authorization header removed from HttpClient");
    }


    #region Private Methods
    /// <summary>
    /// Automatically add token from session (with safety checks)
    /// </summary>
    private void _AddAuthorizationHeader()
    {
        var token = _tokenProvider.GetAccessToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);

            if (apiResponse is not null)
                return apiResponse;

            return ApiResponse<T>.InternalServerError("Empty response.");

        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize response");

            return ApiResponse<T>.InternalServerError("Invalid response format.");
        }
    }
    private async Task<HttpResponseMessage> SendAsync(Func<Task<HttpResponseMessage>> request)
    {
        // ✅ إضافة Authorization Header قبل كل Request
        _AddAuthorizationHeader();

        var response = await request();

        // ✅ لو 401 Unauthorized، حاول تجديد Token
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogWarning("Received 401 Unauthorized, attempting to refresh token");

            var refreshed = await _tokenRefreshService.RefreshAccessTokenAsync();

            if (refreshed)
            {
                // ✅ أضف الـ Token الجديد
                _AddAuthorizationHeader();
                response = await request();
            }
            else
            {
                _logger.LogWarning("Token refresh failed, user needs to login again");
                
                // ✅ لو فشل التجديد، امسح الـ Token وارجع 401
                _tokenProvider.RemoveAccessToken();
                _refreshTokenProvider.RemoveRefreshToken();
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        return response;
    }
    #endregion
}