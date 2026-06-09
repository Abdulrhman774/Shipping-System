using Domain.Entities;
using BL.DTOs.UserSender;

namespace BL.Contract.IServices;

public interface IUserSenderService 
    : IBaseService<TbUserSender, UserSenderDto, CreateUserSenderDto, UpdateUserSenderDto> { }
