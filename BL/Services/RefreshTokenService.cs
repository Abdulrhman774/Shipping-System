using BL.Contract.IServices;
using BL.DTOs.RefreshToken;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IBaseMapper _mapper;
    private readonly IRefreshTokenRepository _refreshRepo;

    public RefreshTokenService(IRefreshTokenRepository refreshRepo, IBaseMapper mapper)
    {
        _refreshRepo = refreshRepo; 
        _mapper = mapper;
    }

    public async Task<RefreshTokenDto?> GetTokenAsync(string token)
    {
        var refreshToken = await _refreshRepo.GetFirstOrDefaultAsync(rt => rt.Token == token);

        if (refreshToken == null)
            return null;


        return _mapper.Map<TbRefreshToken, RefreshTokenDto>(refreshToken);
    }

    public async Task<string?> GetRefreshTokenByUserIdAsync(string userId)
    {
        return (await _refreshRepo.GetFirstOrDefaultAsync(rt => rt.UserId == userId))?.Token;
    }

    public async Task<bool> SaveTokenAsync(string userId, string token, DateTime expires)
    {
        var entity = new TbRefreshToken
        {
            Token = token,
            UserId = userId,
            Expires = expires,
            CreatedDate = DateTime.UtcNow,
            CurrentState = enEntityState.Active
        };

        return await _refreshRepo.CreateAsync(entity, AutoSave: true) != Guid.Empty;
    }

    public async Task<bool> RevokeTokensAsync(string userId)
    {
        return await _refreshRepo.RevokeTokensAsync(userId) > 0;
    }

    public async Task<bool> RevokeTokenAsync(string userId, string token)
    {
        var RevokedToken = await _refreshRepo.GetFirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == token);

        if (RevokedToken == null || RevokedToken.CurrentState is not enEntityState.Active) return false;

        return await _refreshRepo.ChangeStatusAsync(RevokedToken.Id, Guid.Parse(userId), enEntityState.Inactive, AutoSave: true);
    }
    
}
