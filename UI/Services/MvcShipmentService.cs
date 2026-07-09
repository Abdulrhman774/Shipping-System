using BL.Common;
using BL.DTOs.City;
using BL.DTOs.PaymentMethod;
using BL.DTOs.Shipment;
using BL.DTOs.ShippingPackaging;
using BL.DTOs.ShippingType;
using BL.DTOs.UserReceiver;
using BL.DTOs.UserSender;
using UI.Endpoints;

namespace UI.Services
{
    public class MvcShipmentService(GenericApiClient apiClient)
    {
        public Task<ApiResponse<Guid>> CreateShipmentAsync(CreateShipmentDto dto)
        => apiClient.PostAsync<Guid>(stShipmentEndpoints.Create, dto);

        public Task<ApiResponse<Guid>> CreateSenderAsync(CreateUserSenderDto dto)
            => apiClient.PostAsync<Guid>(stLookupEndpoints.UserSenders, dto);

        public Task<ApiResponse<Guid>> CreateReceiverAsync(CreateUserReceiverDto dto)
            => apiClient.PostAsync<Guid>(stLookupEndpoints.UserReceivers, dto);

        public Task<ApiResponse<List<CityDto>>> GetCitiesAsync()
            => apiClient.GetAsync<List<CityDto>>(stLookupEndpoints.Cities);

        public Task<ApiResponse<List<ShippingTypeDto>>> GetShippingTypesAsync()
            => apiClient.GetAsync<List<ShippingTypeDto>>(stLookupEndpoints.ShippingTypes);

        public Task<ApiResponse<List<ShippingPackagingDto>>> GetShippingPackagingAsync()
            => apiClient.GetAsync<List<ShippingPackagingDto>>(stLookupEndpoints.ShippingPackaging);

        public Task<ApiResponse<List<PaymentMethodDto>>> GetPaymentMethodsAsync()
            => apiClient.GetAsync<List<PaymentMethodDto>>(stLookupEndpoints.PaymentMethods);
    }
}
