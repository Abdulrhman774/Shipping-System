using BL.Common;
using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.User;
using System.Security.Claims;
using UI.Endpoints;

namespace UI.Services;

public class UserService : IUserService
{
    private readonly GenericApiClient _apiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        GenericApiClient apiClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _apiClient = apiClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<Result> DeleteAccountAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        return await _apiClient.GetAsync<IEnumerable<UserDto>>(stUserEndpoints.GetAll);
    }

    public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid id)
    {
        return await _apiClient.GetAsync<UserDto>($"{stUserEndpoints.GetById}/{id}");
    }

    public Task<Result<UserDto>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> GetLoggedInUserAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            return Guid.Empty;

        // جيب الـ UserId من الـ Claims
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Guid.Empty;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    public async Task<ApiResponse<UserDto>> GetUserByEmailOrUsernameAsync(string search)
    {
        return await _apiClient.GetAsync<UserDto>(
            $"{stUserEndpoints.Search}?emailOrUsername={search}");
    }

    public Task<Result> UpdateAsync(string updatedUserId, UpdateUserDto dto)
    {
        throw new NotImplementedException();
    }

    Task<Result<IEnumerable<UserDto>>> IUserService.GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    Task<Result<UserDto>> IUserService.GetUserByEmailOrUsernameAsync(string emailOrUsername)
    {
        throw new NotImplementedException();
    }

    //public async Task<ApiResponse> UpdateAsync(UpdateUserDto dto)
    //{
    //    return await apiClient.PutAsync(stUserEndpoints.Update, dto);
    //}

    //public async Task<ApiResponse> DeleteAccountAsync(Guid id)
    //{
    //    return await apiClient.DeleteAsync(stUserEndpoints.Delete, id);
    //}
}