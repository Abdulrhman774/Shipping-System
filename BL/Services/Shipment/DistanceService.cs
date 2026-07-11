// BL/Services/DistanceService.cs

using BL.Contract.IServices;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class DistanceService : IDistanceService
{
    private readonly IGenericRepository<TbCity> _cityRepository;
    private readonly IGenericRepository<TbUserSender> _senderRepository;
    private readonly IGenericRepository<TbUserReceiver> _receiverRepository;

    private const decimal SameCityBaseDistance = 10m;
    private const decimal DifferentCityFallbackDistance = 150m;

    public DistanceService(
        IGenericRepository<TbCity> cityRepository,
        IGenericRepository<TbUserSender> senderRepository,
        IGenericRepository<TbUserReceiver> receiverRepository)
    {
        _cityRepository = cityRepository;
        _senderRepository = senderRepository;
        _receiverRepository = receiverRepository;
    }

    public async Task<decimal> GetDistanceBetweenCitiesAsync(Guid fromCityId, Guid toCityId)
    {
        // Same city
        if (fromCityId == toCityId)
            return SameCityBaseDistance;

        // TODO: Implement actual distance calculation
        // Option 1: Call Google Maps Distance Matrix API
        // Option 2: Query a pre-populated distance table in the database

        // For now, return fallback distance
        return DifferentCityFallbackDistance;
    }

    public async Task<decimal> GetDistanceBetweenSenderAndReceiverAsync(Guid senderId, Guid receiverId)
    {
        var sender = await _senderRepository.GetByIdAsync(senderId);
        var receiver = await _receiverRepository.GetByIdAsync(receiverId);

        if (sender is null || receiver is null)
            return DifferentCityFallbackDistance;

        return await GetDistanceBetweenCitiesAsync(sender.CityId, receiver.CityId);
    }
}