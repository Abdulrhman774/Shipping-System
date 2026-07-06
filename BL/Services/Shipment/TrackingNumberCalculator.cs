using BL.Contract.IServices.Shipment;
using DAL.Contracts;
using Domain.Entities;

namespace BL.Services.Shipment;

public class TrackingNumberCalculator : ITrackingNumberCalculator
{
    private readonly IGenericRepository<TbShipment> _shipmentRepository;
    private const int MaxRetries = 5;

    public TrackingNumberCalculator(IGenericRepository<TbShipment> shipmentRepository)
    {
        _shipmentRepository = shipmentRepository;
    }

    public async Task<string> GenerateTrackingNumber()
    {
        for (int attempt = 0; attempt < MaxRetries; attempt++)
        {
            var candidate = BuildCandidate();

            var exists = await _shipmentRepository
                .ExistsAsync(s => s.TrackingNumber == candidate);

            if (!exists)
                return candidate;
        }

        throw new InvalidOperationException(
            $"Failed to generate a unique tracking number after {MaxRetries} attempts.");
    }

    private static string BuildCandidate()
    {
        // Generate a tracking number in the format "EG" + number digits
        var random = Random.Shared.Next(100_000_000, 999_999_999);
        return $"EG{random}";
    }
}