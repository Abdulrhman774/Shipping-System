using AutoMapper;
using Domain.Entities;
using BL.DTOs.Carrier;
using BL.DTOs.City;
using BL.DTOs.Country;
using BL.DTOs.Log;
using BL.DTOs.PaymentMethod;
using BL.DTOs.Setting;
using BL.DTOs.ShippingType;
using BL.DTOs.Shipment;
using BL.DTOs.ShippingPackaging;
using BL.DTOs.SubscriptionPackage;
using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using BL.DTOs.UserSubscription;
using Domain.Entities.Views;
using BL.DTOs.Views;
using BL.DTOs.RefreshToken;
using BL.DTOs.ShipmentStatus;

namespace BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Log
            CreateMap<Log, LogDto>();
            CreateMap<CreateLogDto, Log>();
            CreateMap<UpdateLogDto, Log>();

            // TbCarrier
            CreateMap<TbCarrier, CarrierDto>();
            CreateMap<CreateCarrierDto, TbCarrier>();
            CreateMap<UpdateCarrierDto, TbCarrier>();

            // TbCity
            CreateMap<TbCity, CityDto>();
            CreateMap<CreateCityDto, TbCity>();
            CreateMap<UpdateCityDto, TbCity>();

            // TbCountry
            CreateMap<TbCountry, CountryDto>();
            CreateMap<CreateCountryDto, TbCountry>();
            CreateMap<UpdateCountryDto, TbCountry>();

            // TbPaymentMethod
            CreateMap<TbPaymentMethod, PaymentMethodDto>();
            CreateMap<CreatePaymentMethodDto, TbPaymentMethod>();
            CreateMap<UpdatePaymentMethodDto, TbPaymentMethod>();

            // TbSetting
            CreateMap<TbSetting, SettingDto>();
            CreateMap<CreateSettingDto, TbSetting>();
            CreateMap<UpdateSettingDto, TbSetting>();

            // TbShippingType
            CreateMap<TbShippingType, ShippingTypeDto>();
            CreateMap<CreateShippingTypeDto, TbShippingType>();
            CreateMap<UpdateShippingTypeDto, TbShippingType>();

            // TbShipment
            CreateMap<TbShipment, ShipmentDto>();
            CreateMap<CreateShipmentDto, TbShipment>();
            CreateMap<UpdateShipmentDto, TbShipment>();

            // TbShipmentStatus
            CreateMap<TbShipmentStatus, ShipmentStatusDto>();
            CreateMap<CreateShipmentStatusDto, TbShipmentStatus>();
            CreateMap<UpdateShipmentStatusDto, TbShipmentStatus>();

            // TbSubscriptionPackage
            CreateMap<TbSubscriptionPackage, SubscriptionPackageDto>();
            CreateMap<CreateSubscriptionPackageDto, TbSubscriptionPackage>();
            CreateMap<UpdateSubscriptionPackageDto, TbSubscriptionPackage>();

            // TbUserReceiver
            CreateMap<TbUserReceiver, UserReceiverDto>();
            CreateMap<CreateUserReceiverDto, TbUserReceiver>();
            CreateMap<UpdateUserReceiverDto, TbUserReceiver>();

            // TbUserSender
            CreateMap<TbUserSender, UserSenderDto>();
            CreateMap<CreateUserSenderDto, TbUserSender>();
            CreateMap<UpdateUserSenderDto, TbUserSender>();

            // TbUserSubscription
            CreateMap<TbUserSubscription, UserSubscriptionDto>();
            CreateMap<CreateUserSubscriptionDto, TbUserSubscription>();
            CreateMap<UpdateUserSubscriptionDto, TbUserSubscription>();

            // TbShippingPackaging
            CreateMap<TbShippingPackaging, ShippingPackagingDto>();
            CreateMap<CreateShippingPackagingDto, TbShippingPackaging>();
            CreateMap<UpdateShippingPackagingDto, TbShippingPackaging>();

            // TbRefreshToken
            CreateMap<TbRefreshToken, RefreshTokenDto>().ReverseMap();


            // Views
            CreateMap<VwCitiesCountries, VwCitiesCountriesDto>();
            
        }
    }
}
