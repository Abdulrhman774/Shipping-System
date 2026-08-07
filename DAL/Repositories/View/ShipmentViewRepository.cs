using DAL.Context;
using DAL.Contracts.IRepositories;
using Domain.Entities.Views.Dashboard;
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


        public async Task<(int TotalShipments, int ActiveDeliveries, int PendingApprovals, int DelayedShipments, List<vw_ShipmentDetails> RecentShipments)> 
            GetDashboardStatsAsync()
        {
            var query = _context.VwShipmentDetails.AsNoTracking();

            var total = await query.CountAsync();

            // تعريف الحالات حسب الـ enum الجديد (افترض أن Shipped = 4, Created = 1, Approved = 2)
            var active = await query.CountAsync(x => x.Status == enShipmentStatus.Shipped);
            var pending = await query.CountAsync(x => x.Status == enShipmentStatus.Created || x.Status == enShipmentStatus.Approved);

            // يمكنك حساب المتأخرات إذا كان لديك حقل DeliveryDate أو ExpectedDate
            // حالياً نضع 0
            var delayed = 0;

            var recent = await query
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToListAsync();

            return (total, active, pending, delayed, recent);
        }


        public async Task<VwDashboardSummary?> GetDashboardSummaryAsync()
        {
            return await _context.Database
                .SqlQueryRaw<VwDashboardSummary>("SELECT * FROM vw_DashboardSummary")
                .FirstOrDefaultAsync();
        }

        public async Task<List<VwRecentShipment>> GetRecentShipmentsAsync(int count = 20)
        {
            return await _context.Database
                .SqlQueryRaw<VwRecentShipment>("SELECT * FROM vw_RecentShipments")
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<VwShipmentStatusDistribution>> GetStatusDistributionAsync()
        {
            return await _context.Database
                .SqlQueryRaw<VwShipmentStatusDistribution>("SELECT * FROM vw_ShipmentStatusDistribution")
                .ToListAsync();
        }

        public async Task<List<VwMonthlyVolume>> GetMonthlyVolumeAsync()
        {
            return await _context.Database
                .SqlQueryRaw<VwMonthlyVolume>("SELECT * FROM vw_MonthlyVolume ORDER BY [Year], [Month]")
                .ToListAsync();
        }

        public async Task<List<VwTopShipper>> GetTopShippersAsync(int top = 5)
        {
            return await _context.Database
                .SqlQueryRaw<VwTopShipper>("SELECT * FROM vw_TopShippers")
                .Take(top)
                .ToListAsync();
        }

        public async Task<List<VwFinancial>> GetMonthlyFinancialsAsync()
        {
            return await _context.Database
                .SqlQueryRaw<VwFinancial>("SELECT * FROM vw_Financials ORDER BY [Year], [Month]")
                .ToListAsync();
        }

    }

}