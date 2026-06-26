using BL.Common;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace UI.Services;

/// <summary>
/// A generic HTTP client service that provides a simplified interface for making API calls.
/// This class abstracts the complexity of HttpClient and provides type-safe methods
/// for common HTTP operations (GET, POST, PUT, DELETE).
/// </summary>
public class GenericApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<GenericApiClient> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericApiClient"/> class.
    /// </summary>
    public GenericApiClient(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<GenericApiClient> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        _httpClient = httpClientFactory.CreateClient("ApiClient");
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        var baseUrl = configuration["ApiSettings:BaseUrl"]
            ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");

        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    /// <summary>
    /// Set authorization header with Bearer token
    /// </summary>
    public void SetAuthorizationHeader(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    /// <summary>
    /// Clear authorization header
    /// </summary>
    public void ClearAuthorizationHeader()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }


    /// <summary>
    /// Sends an asynchronous GET request to the specified endpoint and returns ApiResponse{T}.
    /// </summary>
    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
        _AddTokenFromSession();

        try
        {
            var response = await _httpClient.GetAsync(endpoint);
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
        _AddTokenFromSession();

        try
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST request failed for {Endpoint}", endpoint);
            return ApiResponse<T>.BadRequest($"Request failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends an asynchronous PUT request to the specified endpoint and returns ApiResponse{T}.
    /// </summary>
    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
    {
        _AddTokenFromSession();

        try
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync(endpoint, content);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT request failed for {Endpoint}", endpoint);
            return ApiResponse<T>.BadRequest($"Request failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends an asynchronous DELETE request to the specified endpoint and returns ApiResponse{T}.
    /// </summary>
    public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
    {
        _AddTokenFromSession();

        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return await ProcessResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DELETE request failed for {Endpoint}", endpoint);
            return ApiResponse<T>.BadRequest($"Request failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Sends an asynchronous DELETE request without expecting a response body.
    /// </summary>
    public async Task<bool> DeleteAsync(string endpoint)
    {
        _AddTokenFromSession();

        try
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DELETE request failed for {Endpoint}", endpoint);
            return false;
        }
    }

    #region Private Methods
    /// <summary>
    /// Automatically add token from session (with safety checks)
    /// </summary>
    private void _AddTokenFromSession()
    {
        try
        {
            // ✅ Check for HttpContext
            if (_httpContextAccessor.HttpContext == null)
                return;

            // ✅ Check for Session
            if (_httpContextAccessor.HttpContext.Session == null)
                return;

            // ✅ Attempt to read the token
            var token = _httpContextAccessor.HttpContext.Session.GetString("AccessToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to get token from session");
        }
    }

    private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            // ✅ Deserialize مباشرة إلى ApiResponse<T>
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);

            // ✅ إذا كان الـ Response Success، نرجع ApiResponse
            if (response.IsSuccessStatusCode && apiResponse != null)
                return apiResponse;

            // ❌ إذا فشل، نبني ApiResponse من StatusCode
            var errorMsg = apiResponse?.Error ?? GetDefaultErrorMessage(response.StatusCode);

            return response.StatusCode switch
            {
                HttpStatusCode.NotFound => ApiResponse<T>.NotFound(errorMsg),
                HttpStatusCode.Unauthorized => ApiResponse<T>.Unauthorized(errorMsg),
                HttpStatusCode.Forbidden => ApiResponse<T>.Forbidden(errorMsg),
                HttpStatusCode.BadRequest => ApiResponse<T>.BadRequest(errorMsg),
                _ => ApiResponse<T>.InternalServerError(errorMsg)
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize response");

            if (response.IsSuccessStatusCode)
                return ApiResponse<T>.Ok(default);

            return ApiResponse<T>.BadRequest("Invalid response format");
        }
    }
    private static string GetDefaultErrorMessage(HttpStatusCode statusCode)
    {
        return statusCode switch
        {
            HttpStatusCode.NotFound => "Resource not found",
            HttpStatusCode.Unauthorized => "Unauthorized access",
            HttpStatusCode.Forbidden => "Access forbidden",
            HttpStatusCode.BadRequest => "Invalid request",
            _ => "An error occurred"
        };
    }

    #endregion
}