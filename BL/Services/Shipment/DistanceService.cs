using BL.Contract.IServices;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services;

public class DistanceService : IDistanceService
{
    private const decimal SameCityBaseDistance = 10m;
    private const decimal DifferentCityFallbackDistance = 150m;

    public Task<decimal> GetDistanceBetweenCitiesAsync(Guid fromCityId, Guid toCityId)
    {
        if (fromCityId == toCityId)
            return Task.FromResult(SameCityBaseDistance);

        // TODO: real calculation
        return Task.FromResult(DifferentCityFallbackDistance);
    }
}