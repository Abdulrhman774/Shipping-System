using BL.Common;
using Newtonsoft.Json;
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
            if (response.IsSuccessStatusCode)
            {
                var directData = JsonConvert.DeserializeObject<T>(responseContent);
                if (directData != null)
                {
                    // Make sure T is not just an empty object (optional)
                    // If T is a LoginResponseDto, make sure there is an AccessToken
                    return ApiResponse<T>.Ok(directData);
                }
            }


            // Try to deserialize as ApiResponse<T>
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent);

            if (apiResponse != null)
            {
                // If it's a successful response
                if (response.IsSuccessStatusCode && apiResponse.Success)
                {
                    return apiResponse;
                }

                // If it's an error response
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = apiResponse.Error ?? $"HTTP Error: {response.StatusCode}";

                    return response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => ApiResponse<T>.NotFound(errorMessage),
                        System.Net.HttpStatusCode.Unauthorized => ApiResponse<T>.Unauthorized(errorMessage),
                        System.Net.HttpStatusCode.Forbidden => ApiResponse<T>.Forbidden(errorMessage),
                        System.Net.HttpStatusCode.BadRequest => ApiResponse<T>.BadRequest(errorMessage),
                        _ => ApiResponse<T>.BadRequest(errorMessage)
                    };
                }

                if (apiResponse.Data != null)
                    return ApiResponse<T>.Ok(apiResponse.Data);

                // وإلا نعيد BadRequest
                return ApiResponse<T>.BadRequest("Invalid response format");
            }

            // If deserialization failed but status is success, try to deserialize as T directly
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<T>(responseContent);
                return ApiResponse<T>.Ok(data);
            }

            // Handle error based on status code
            var errorMsg = $"HTTP Error: {response.StatusCode}";
            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.NotFound => ApiResponse<T>.NotFound(errorMsg),
                System.Net.HttpStatusCode.Unauthorized => ApiResponse<T>.Unauthorized(errorMsg),
                System.Net.HttpStatusCode.Forbidden => ApiResponse<T>.Forbidden(errorMsg),
                _ => ApiResponse<T>.BadRequest(errorMsg)
            };
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize response from {Endpoint}", response.RequestMessage?.RequestUri);

            if (response.IsSuccessStatusCode)
            {
                return ApiResponse<T>.Ok(default);
            }

            return ApiResponse<T>.BadRequest($"Failed to parse response: {ex.Message}");
        }
    }

    #endregion
}