using BL.Contract.IServices;
using BL.DTOs.Setting;
using BL.DTOs.ShippingType;
using BL.Mapping;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class SettingService
    : BaseService<TbSetting, SettingDto, CreateSettingDto, UpdateSettingDto>, ISettingService
{
    public SettingService(IGenericRepository<TbSetting> repository, IMapper mapper, IUserService userService)
        : base(repository, mapper, userService) { }
}