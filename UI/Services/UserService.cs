using BL.Common;
using BL.Common.Results;
using BL.Contract.IServices;
using BL.DTOs.User;
using System.Security.Claims;
using UI.Endpoints;

namespace UI.Services;

public class UserService(GenericApiClient apiClient, HttpContextAccessor _httpContextAccessor) : IUserService
{
    

    public Task<Result> DeleteAccountAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        return await apiClient.GetAsync<IEnumerable<UserDto>>(stUserEndpoints.GetAll);
    }

    public async Task<ApiResponse<UserDto>> GetByIdAsync(Guid id)
    {
        return await apiClient.GetAsync<UserDto>($"{stUserEndpoints.GetById}/{id}");
    }

    public Task<Result<UserDto>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> GetLoggedInUserAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty;
    }

    public async Task<ApiResponse<UserDto>> GetUserByEmailOrUsernameAsync(string search)
    {
        return await apiClient.GetAsync<UserDto>(
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