using BL.DTOs.RefreshToken;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract.IServices;

public interface IRefreshTokenService
{
    Task<RefreshTokenDto?> GetTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string userId);
    Task<bool> SaveTokenAsync(string userId, string token, DateTime expires);
    Task<bool> RevokeTokenAsync(string userId, string token);
}