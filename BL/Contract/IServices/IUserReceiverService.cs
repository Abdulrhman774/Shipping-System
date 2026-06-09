using Domain.Entities;
using BL.DTOs.UserReceiver;

namespace BL.Contract.IServices;

public interface IUserReceiverService 
    : IBaseService<TbUserReceiver, UserReceiverDto, CreateUserReceiverDto, UpdateUserReceiverDto> { }
