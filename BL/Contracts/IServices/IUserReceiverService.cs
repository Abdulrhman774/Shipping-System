using Domain.Entities;
using BL.DTOs.UserReceiver;

namespace BL.Contracts.IServices;

public interface IUserReceiverService 
    : IBaseService<TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto> { }
