using DAL.Context;
using DAL.Contracts.IRepositories;
using Domain.Entities.Views.Shipment.Statistics_Views;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.View
{
    public class ShipmentViewRepository : IShipmentViewRepository
    {
        private readonly ShippingDbContext _context;

        public ShipmentViewRepository(ShippingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<vw_ShipmentDetails>> GetShipmentDetailsAsync()
        {
            return await _context.VwShipmentDetails
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<vw_ShipmentDetails?> GetShipmentDetailByIdAsync(Guid id)
        {
            return await _context.VwShipmentDetails
                .FirstOrDefaultAsync(x => x.ShipmentId == id);
        }

        public async Task<IEnumerable<vw_ShipmentDetails>> GetShipmentsByUserAsync(Guid userId)
        {
            return await _context.VwShipmentDetails
                .Where(x => x.SenderId == userId
                            || x.ReceiverId == userId
                            || x.CreatedBy == userId)  
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<vw_ShipmentDetails?> GetShipmentByTrackingNumberAsync(string trackingNumber, Guid CreatedByUserId)
        {
            return await _context.VwShipmentDetails
                .FirstOrDefaultAsync(x => x.TrackingNumber == trackingNumber && x.CreatedBy == CreatedByUserId);
        }

        public async Task<IEnumerable<vw_ShipmentStats>> GetShipmentStatsAsync()
        {
            return await _context.VwShipmentStats.ToListAsync();
        }

        public async Task<IEnumerable<VwMonthlyShipments>> GetMonthlyShipmentsAsync()
        {
            return await _context.VwMonthlyShipments
                .OrderByDescending(x => x.ShipmentYear)
                .ThenByDescending(x => x.ShipmentMonth)
                .ToListAsync();
        }

        public async Task<IEnumerable<VwShipmentsByType>> GetShipmentsByTypeAsync()
        {
            return await _context.VwShipmentsByType
                .OrderByDescending(x => x.ShipmentCount)
                .ToListAsync();
        }
        public async Task<(IEnumerable<vw_ShipmentDetails> Data, int TotalCount)> GetPagedShipmentDetailsAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        {
            var query = _context.VwShipmentDetails
                .OrderByDescending(x => x.CreatedDate)
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (data, totalCount);
        }

        public async Task<(IEnumerable<vw_ShipmentDetails> Data, int TotalCount)> GetShipmentsByUserPagedAsync(
        int pageNumber,
        int pageSize,
        Guid userId,
        CancellationToken cancellationToken = default)
        {
            // ✅ 1. ابدأ بـ IQueryable
            var query = _context.VwShipmentDetails
                .AsNoTracking(); // لا تحسب العدد بعد

            // ✅ 2. طبق الفلتر أولاً
            query = query.Where(x => x.CreatedBy == userId);

            // ✅ 3. احسب العدد الإجمالي (بعد الفلتر)
            var totalCount = await query.CountAsync(cancellationToken);

            // ✅ 4. طبق الترتيب والتصفح
            var data = await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (data, totalCount);
        }
    }
}