using BL.DTOs.Setting;
using Domain.Entities;

namespace BL.Contract.IServices;

public interface ISettingService : IBaseService<TbSetting, SettingDto, CreateSettingDto, UpdateSettingDto> { }
