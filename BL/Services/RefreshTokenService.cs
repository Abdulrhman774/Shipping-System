using BL.Contract.IServices;
using BL.DTOs.RefreshToken;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<TbRefreshToken> _refreshRepo;

    public RefreshTokenService(IGenericRepository<TbRefreshToken> refreshRepo, IMapper mapper)
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
        return await _refreshRepo.AddAsync(entity);
    }

    public async Task<bool> RevokeTokenAsync(string userId)
    {
        var tokenList = await  _refreshRepo.GetListAsync(rt => rt.UserId == userId);

        foreach (var token in tokenList)
        {
            if (token.CurrentState is enEntityState.Active)
                await _refreshRepo.ChangeStatusAsync(token.Id, Guid.Parse(userId), enEntityState.Inactive);

        }

        return true;
    }

    public async Task<bool> RevokeTokenAsync(string userId, string token)
    {
        var RevokedToken = await _refreshRepo.GetFirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == token);

        if (RevokedToken.CurrentState is not enEntityState.Active) return false;

        return await _refreshRepo.ChangeStatusAsync(RevokedToken.Id, Guid.Parse(userId), enEntityState.Inactive);
    }
}
