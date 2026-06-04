using Domain.Entities;
using BL.DTOs.UserSender;

namespace BL.Contracts.IServices;

public interface IUserSenderService 
    : IBaseService<TbUserSender, UserSenderDto, CreateUserSenderDto, UpdateUserSenderDto> { }
